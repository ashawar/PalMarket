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

        // GET: api/store?deviceCode=1002
        public IHttpActionResult GetStores(string deviceCode)
        {
            IEnumerable<StoreDTO> storesDTO;
            IEnumerable<DetailedStore> stores;

            stores = storeService.GetStores(deviceCode);

            //string baseImageUrl = Utilities.GetBaseUrl() + "/api/Store/Image/";
            string baseImageUrl = Utilities.GetBaseUrl() + "/file.asmx/StoreImage?id=";

            storesDTO = stores.Select(a => new StoreDTO
            {
                StoreID = a.StoreID,
                Name = a.Name,
                QRCode = a.QRCode,
                IsSubscribed = a.IsSubscribed,
                Branches = a.Branches.Select(b => new BranchDTO { BranchID = b.BranchID, Location = b.Location, City = b.City != null ? b.City.Name : null, CityID = b.CityID, Longitude = b.Longitude, Latitude = b.Latitude, Store = b.Store != null ? b.Store.Name : null, StoreID = b.StoreID }),
                ImageUrl = a.Image != null ? baseImageUrl + a.StoreID : null,
                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            });


            return Ok(storesDTO);
        }

        // GET: api/store/5
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

            //string baseImageUrl = Utilities.GetBaseUrl() + "/api/Store/Image/";
            string baseImageUrl = Utilities.GetBaseUrl() + "/file.asmx/StoreImage?id=";
            storeDTO.ImageUrl = store.ImageFileName != null ? baseImageUrl + storeDTO.StoreID : null;

            return Ok(storeDTO);
        }

        // POST: api/store/image/5
        [HttpPost]
        [Route("image/{id}")]
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

            // Fill the file name and size 
            store.ImageFileName = file.FileName;
            store.ImageFileSize = file.ContentLength;

            // Save the record
            storeService.UpdateStore(store);
            storeService.SaveStore();

            // If the record has been saved successfully then save the file
            string path = ConfigHelper.GetDocumentsFolderPath() + "Stores/Images/";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            path += id.ToString();

            file.SaveAs(path);

            //string imageUrl = Utilities.GetBaseUrl() + "/api/store/image/" + id;
            string imageUrl = Utilities.GetBaseUrl() + "/file.asmx/StoreImage?id=" + id;

            return Created(imageUrl, new { ImageUrl = imageUrl });
        }

        // GET: api/store/Image/5
        [HttpGet]
        [Route("image/{id}")]
        public void DownloadImage(int id)
        {
            // Get the store record
            Store store = storeService.GetStore(id);
            string path = ConfigHelper.GetDocumentsFolderPath() + "Stores/Images/" + id;

            // Write the file directly to the HTTP content output stream.
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(store.ImageFileName);
            HttpContext.Current.Response.WriteFile(path);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + store.ImageFileName);
            HttpContext.Current.Response.AddHeader("FileName", store.ImageFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", store.ImageFileSize.ToString());
            HttpContext.Current.Response.Flush();

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(store.ImageFileName);
            //HttpContext.Current.Response.OutputStream.Write(store.Image, 0, store.Image.Length);
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + store.ImageFileName);
            //HttpContext.Current.Response.AddHeader("Content-Length", store.Image.Length.ToString());
            //HttpContext.Current.Response.End();
        }
    }
}