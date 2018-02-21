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
using System.Web;
using PalMarket.Common.Helpers;
using System.IO;
using System.Threading.Tasks;

namespace PalMarket.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/notification")]
    [ControllerExceptionFilter]
    public class NotificationController : ApiController
    {
        private readonly INotificationService notificationService;
        private readonly IDeviceService deviceService;

        public NotificationController(INotificationService notificationService, IDeviceService deviceService)
        {
            this.notificationService = notificationService;
            this.deviceService = deviceService;
        }

        // POST: api/notification/general
        [HttpPost]
        [Route("general")]
        public async Task<IHttpActionResult> SendGeneralNotification(NotificationDTO notificationDTO)
        {
            int branchID = this.getBranchID();
            List<Device> devices = deviceService.GetByBranch(branchID);
            notificationService.SendNotification(devices, notificationDTO.Message);

            return Ok();
        }

        private int getBranchID()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            return Convert.ToInt32(identity.Claims.AsQueryable().FirstOrDefault(a => a.Type == "BranchID").Value);
        }
    }
}