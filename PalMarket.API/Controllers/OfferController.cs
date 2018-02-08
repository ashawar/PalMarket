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

        // GET: api/Offer
        public IHttpActionResult GetOffers()
        {
            IEnumerable<OfferDTO> offersDTO;
            IEnumerable<Offer> offers;

            int branchID = this.getBranchID();

            offers = offerService.GetBranchOffers(branchID);

            string baseImageUrl = Utilities.GetBaseUrl() + "/api/Offer/Image?id=";
            offersDTO = offers.Select(a => new OfferDTO
            {
                OfferID = a.OfferID,
                Name = a.Name,
                OldPrice = a.OldPrice,
                NewPrice = a.NewPrice,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                ImageUrl = a.Image != null ? baseImageUrl + a.OfferID : null,
                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            });

            return Ok(offersDTO);
        }

        // GET: api/Offer?branchID=1
        [AllowAnonymous]
        public IHttpActionResult GetOffers(int branchID)
        {
            IEnumerable<OfferDTO> offersDTO;
            IEnumerable<Offer> offers;

            offers = offerService.GetBranchOffers(branchID);

            string baseImageUrl = Utilities.GetBaseUrl() + "/api/Offer/Image?id=";
            offersDTO = offers.Select(a => new OfferDTO
            {
                OfferID = a.OfferID,
                Name = a.Name,
                OldPrice = a.OldPrice,
                NewPrice = a.NewPrice,
                DateStart = a.DateStart,
                DateEnd = a.DateEnd,
                ImageUrl = a.Image != null ? baseImageUrl + a.OfferID : null,
                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            });

            return Ok(offersDTO);
        }

        // GET: api/Offer/5
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
            string baseImageUrl = Utilities.GetBaseUrl() + "/api/Offer/Image?id=";
            offerDTO.ImageUrl = offer.Image != null ? baseImageUrl + offerDTO.OfferID : null;

            return Ok(offerDTO);
        }

        // POST: api/Offer/Image?id=5
        [HttpPost]
        [Route("Image")]
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
            offer.ImageFileName = file.FileName;

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                offer.Image = binaryReader.ReadBytes(file.ContentLength);
            }

            offerService.UpdateOffer(offer);
            offerService.SaveOffer();

            return Ok();
        }

        // GET: api/Offer/Image?id=5
        [HttpGet]
        [Route("Image")]
        [AllowAnonymous]
        public void DownloadImage(int id)
        {
            Offer offer = offerService.GetOffer(id);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(offer.ImageFileName);
            HttpContext.Current.Response.OutputStream.Write(offer.Image, 0, offer.Image.Length);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + offer.ImageFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", offer.Image.Length.ToString());
            HttpContext.Current.Response.End();
        }

        // POST: api/Offer
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

        // PUT: api/Offer/5
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

        // DELETE: api/Offer/5
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