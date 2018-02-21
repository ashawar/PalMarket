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
using System.IO;
using PalMarket.Common.Helpers;
using System.Web;

namespace PalMarket.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/offer")]
    [ControllerExceptionFilter]
    public class OfferController : ApiController
    {
        private readonly IOfferService offerService;

        public OfferController(IOfferService offerService)
        {
            this.offerService = offerService;
        }

        // GET: api/offer
        public IHttpActionResult GetOffers()
        {
            IEnumerable<OfferDTO> offersDTO;
            IEnumerable<Offer> offers;

            int branchID = this.getBranchID();

            offers = offerService.GetBranchOffers(branchID);

            //string baseImageUrl = Utilities.GetBaseUrl() + "/api/Offer/Image/";
            string baseImageUrl = Utilities.GetBaseUrl() + "/file.asmx/OfferImage?id=";

            offersDTO = offers.Select(a => new OfferDTO
            {
                OfferID = a.OfferID,
                Name = a.Name,
                OldPrice = a.OldPrice,
                NewPrice = a.NewPrice,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                ImageUrl = a.ImageFileName != null ? baseImageUrl + a.OfferID : null,
                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            });

            return Ok(offersDTO);
        }

        // GET: api/offer?branchID=1
        [AllowAnonymous]
        public IHttpActionResult GetOffers(int branchID)
        {
            IEnumerable<OfferDTO> offersDTO;
            IEnumerable<Offer> offers;

            offers = offerService.GetBranchOffers(branchID);

            //string baseImageUrl = Utilities.GetBaseUrl() + "/api/Offer/Image/";
            string baseImageUrl = Utilities.GetBaseUrl() + "/file.asmx/OfferImage?id=";

            offersDTO = offers.Select(a => new OfferDTO
            {
                OfferID = a.OfferID,
                Name = a.Name,
                OldPrice = a.OldPrice,
                NewPrice = a.NewPrice,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                ImageUrl = a.ImageFileName != null ? baseImageUrl + a.OfferID : null,
                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            });

            return Ok(offersDTO);
        }

        // GET: api/offer/5
        [ResponseType(typeof(Offer))]
        public IHttpActionResult GetOffer(int id)
        {
            OfferDTO offerDTO;
            Offer offer = offerService.GetOffer(id);

            if (offer == null)
            {
                return NotFound();
            }

            offerDTO = Mapper.Map<Offer, OfferDTO>(offer);
            //string baseImageUrl = Utilities.GetBaseUrl() + "/api/Offer/Image/";
            string baseImageUrl = Utilities.GetBaseUrl() + "/file.asmx/OfferImage?id=";
            offerDTO.ImageUrl = offer.ImageFileName != null ? baseImageUrl + offerDTO.OfferID : null;

            return Ok(offerDTO);
        }

        // POST: api/offer/image/5
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

            Offer offer = offerService.GetOffer(id);

            // Fill the file name and size 
            offer.ImageFileName = file.FileName;
            offer.ImageFileSize = file.ContentLength;

            // Save the record
            offerService.UpdateOffer(offer);
            offerService.SaveOffer();

            // If the record has been saved successfully then save the file
            string path = ConfigHelper.GetDocumentsFolderPath() + "Offers/Images/";

            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            path += id.ToString();

            file.SaveAs(path);

            //string imageUrl = Utilities.GetBaseUrl() + "/api/offer/image/" + offer.OfferID;
            string imageUrl = Utilities.GetBaseUrl() + "/file.asmx/OfferImage?id=" + offer.OfferID;

            return Created(imageUrl, new { ImageUrl = imageUrl });
        }

        // GET: api/offer/image/5
        [HttpGet]
        [Route("image/{id}")]
        [AllowAnonymous]
        public void DownloadImage(int id)
        {
            // Get the offer record
            Offer offer = offerService.GetOffer(id);
            string path = ConfigHelper.GetDocumentsFolderPath() + "Offers/Images/" + id;

            // Write the file directly to the HTTP content output stream.
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(offer.ImageFileName);
            HttpContext.Current.Response.WriteFile(path);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=\"" + offer.ImageFileName + "\"");
            HttpContext.Current.Response.AddHeader("FileName", offer.ImageFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", offer.ImageFileSize.ToString());
            HttpContext.Current.Response.End();

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(offer.ImageFileName);
            //HttpContext.Current.Response.OutputStream.Write(offer.Image, 0, offer.Image.Length);
            //HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + offer.ImageFileName);
            //HttpContext.Current.Response.AddHeader("Content-Length", offer.Image.Length.ToString());
            //HttpContext.Current.Response.End();
        }

        // POST: api/offer
        [ResponseType(typeof(Offer))]
        [ValidateModel]
        public IHttpActionResult PostOffer([FromBody] OfferDTO offerDTO)
        {
            offerDTO.DateCreated = DateTime.UtcNow;
            Offer offer = Mapper.Map<OfferDTO, Offer>(offerDTO);
            offer.BranchID = this.getBranchID();

            //if (offerDTO.File != null)
            //{
            //    if (!Utilities.IsImageExtension(Utilities.GetFileExtension(offerDTO.File.FileName)))
            //    {
            //        return BadRequest("Invalid Image");
            //    }
            //    offer.ImageFileName = offerDTO.File.FileName;

            //    using (var binaryReader = new BinaryReader(offerDTO.File.InputStream))
            //    {
            //        offer.Image = binaryReader.ReadBytes(offerDTO.File.ContentLength);
            //    }
            //}

            offerService.AddOffer(offer);
            offerService.SaveOffer();

            offerDTO.OfferID = offer.OfferID;
            return CreatedAtRoute("DefaultApi", new { id = offer.OfferID }, offerDTO);
        }

        // PUT: api/offer/5
        [ResponseType(typeof(void))]
        [ValidateModel]
        public IHttpActionResult PutOffer(int id, [FromBody] OfferDTO offerDTO)
        {
            if (id != offerDTO.OfferID)
            {
                return BadRequest();
            }

            Offer offer = Mapper.Map<OfferDTO, Offer>(offerDTO);
            Offer offerToUpdate = offerService.GetOffer(id);

            offerToUpdate.Name = offer.Name;
            offerToUpdate.OldPrice = offer.OldPrice;
            offerToUpdate.NewPrice = offer.NewPrice;
            offerToUpdate.DateStart = offer.DateStart;
            offerToUpdate.DateEnd = offer.DateEnd;
            offerToUpdate.DateUpdated = DateTime.UtcNow;

            offerService.UpdateOffer(offerToUpdate);
            offerService.SaveOffer();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/offer/5
        [ResponseType(typeof(Offer))]
        public IHttpActionResult DeleteOffer(int id)
        {
            Offer offer = offerService.GetOffer(id);
            if (offer == null)
            {
                return NotFound();
            }

            offerService.DeleteOffer(offer);
            offerService.SaveOffer();

            OfferDTO offerDTO = Mapper.Map<Offer, OfferDTO>(offer);

            return Ok(offerDTO);
        }

        private int getBranchID()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            return Convert.ToInt32(identity.Claims.AsQueryable().FirstOrDefault(a => a.Type == "BranchID").Value);
        }
    }
}