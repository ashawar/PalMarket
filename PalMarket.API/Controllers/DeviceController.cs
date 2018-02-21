using System;
using System.Web.Http;
using PalMarket.Domain.Contracts.Services;
using PalMarket.API.DTOs;
using PalMarket.Model;
using System.Collections.Generic;
using AutoMapper;
using System.Web.Http.Description;
using System.Net;
using System.Threading;
using System.Security.Claims;
using System.Linq;

namespace PalMarket.API.Controllers
{
    [RoutePrefix("api/device")]
    [ControllerExceptionFilter]
    public class DeviceController : ApiController
    {
        private readonly IDeviceService deviceService;
        private readonly IStoreService storeService;

        public DeviceController(IDeviceService deviceService, IStoreService storeService)
        {
            this.deviceService = deviceService;
            this.storeService = storeService;
        }

        // POST: api/device
        [ResponseType(typeof(Device))]
        [ValidateModel]
        public IHttpActionResult PostDevice([FromBody] DeviceDTO deviceDTO)
        {
            deviceDTO.DateCreated = DateTime.UtcNow;
            deviceDTO.DeviceCode = Guid.NewGuid().ToString();
            Device device = Mapper.Map<DeviceDTO, Device>(deviceDTO);

            deviceService.AddDevice(device);
            deviceService.SaveDevice();

            deviceDTO.DeviceID = device.DeviceID;
            return CreatedAtRoute("DefaultApi", new { id = device.DeviceID }, deviceDTO);
        }

        // POST: api/device/subscribe
        [HttpPost]
        [Route("subscribe")]
        [ResponseType(typeof(Device))]
        [ValidateModel]
        public IHttpActionResult Subscribe([FromBody] SubscribeDTO subscribeDTO)
        {
            var device = deviceService.GetByCode(subscribeDTO.DeviceCode);

            if (device == null)
            {
                return BadRequest();
            }

            var store = storeService.GetByQR(subscribeDTO.StoreQR);

            if (store == null)
            {
                return BadRequest();
            }

            StoreDevice subscribtion = new StoreDevice()
            {
                DeviceID = device.DeviceID,
                StoreID = store.StoreID,
                DateCreated = DateTime.UtcNow
            };

            deviceService.Subscribe(subscribtion);
            deviceService.SaveDevice();

            return Ok();
        }

        // DELETE: api/device/3c81ee32-e672-47a0-98f1-e4dd07e895cb
        [Route("{code}")]
        [ResponseType(typeof(Device))]
        public IHttpActionResult DeleteDevice(string code)
        {
            Device device = deviceService.GetByCode(code);
            if (device == null)
            {
                return NotFound();
            }

            deviceService.DeleteDevice(device);
            deviceService.SaveDevice();

            DeviceDTO deviceDTO = Mapper.Map<Device, DeviceDTO>(device);

            return Ok(deviceDTO);
        }
    }
}