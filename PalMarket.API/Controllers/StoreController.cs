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

namespace PalMarket.API.Controllers
{
    [RoutePrefix("api/store")]
    [ControllerExceptionFilter]
    public class StoreController : ApiController
    {
        private readonly IStoreService storeService;

        public StoreController(IStoreService storeService)
        {
            this.storeService = storeService;
        }

        // GET: api/Store?deviceCode=1002
        public IHttpActionResult GetStores(string deviceCode)
        {
            IEnumerable<StoreDTO> storesDTO;
            IEnumerable<DetailedStore> stores;

            stores = storeService.GetStores(deviceCode);
            storesDTO = Mapper.Map<IEnumerable<DetailedStore>, IEnumerable<StoreDTO>>(stores);

            string baseImageUrl = Utilities.GetBaseUrl() + "/api/Store/Image?id=";
            storesDTO = storesDTO.Select(a => new StoreDTO
            {
                StoreID = a.StoreID,
                Name = a.Name,
                QRCode = a.QRCode,
                IsSubscribed = a.IsSubscribed,
                ImageUrl = baseImageUrl + a.StoreID,
                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            });


            return Ok(storesDTO);
        }

        // GET: api/Store/5
        [ResponseType(typeof(Store))]
        public IHttpActionResult GetStore(int id)
        {
            StoreDTO storeDTO;
            Store store = storeService.GetStore(id);

            if (store == null)
            {
                return NotFound();
            }

            storeDTO = Mapper.Map<Store, StoreDTO>(store);

            string baseImageUrl = Utilities.GetBaseUrl() + "/api/Store/Image?id=";
            storeDTO.ImageUrl = baseImageUrl + storeDTO.StoreID;

            return Ok(storeDTO);
        }

        // POST: api/Store/Image?id=5
        [HttpPost]
        [Route("Image")]
        [AllowAnonymous]
        public IHttpActionResult UploadImage(int id)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            if (file == null)
            {
                return BadRequest("File not submitted");
            }

            if (!Utilities.IsImageExtension(Utilities.GetFileExtension(file.FileName)))
            {
                return BadRequest("Invalid Image");
            }

            Store store = storeService.GetStore(id);
            store.ImageFileName = file.FileName;

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                store.Image = binaryReader.ReadBytes(file.ContentLength);
            }

            storeService.UpdateStore(store);
            storeService.SaveStore();

            return Ok();
        }

        // GET: api/Store/Image?id=5
        [HttpGet]
        [Route("Image")]
        [AllowAnonymous]
        public void DownloadImage(int id)
        {
            Store store = storeService.GetStore(id);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(store.ImageFileName);
            HttpContext.Current.Response.OutputStream.Write(store.Image, 0, store.Image.Length);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + store.ImageFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", store.Image.Length.ToString());
            HttpContext.Current.Response.End();
        }
    }
}