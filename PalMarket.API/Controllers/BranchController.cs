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
    [RoutePrefix("api/branch")]
    [ControllerExceptionFilter]
    public class BranchController : ApiController
    {
        private readonly IBranchService branchService;

        public BranchController(IBranchService branchService)
        {
            this.branchService = branchService;
        }

        // GET: api/Branch/Current
        [Route("Current")]
        [ResponseType(typeof(Branch))]
        public IHttpActionResult GetCurrentBranch()
        {
            int id = this.getBranchID();

            BranchDTO branchDTO;
            Branch branch = branchService.GetBranch(id);

            if (branch == null)
            {
                return NotFound();
            }

            branchDTO = Mapper.Map<Branch, BranchDTO>(branch);

            return Ok(branchDTO);
        }

        // PUT: api/Branch/Current
        [Route("Current")]
        [ResponseType(typeof(void))]
        [ValidateModel]
        public IHttpActionResult PutCurrentBranch([FromBody] BranchSaveDTO branchDTO)
        {
            int id = this.getBranchID();

            if (id != branchDTO.BranchID)
            {
                return BadRequest();
            }

            Branch branchToUpdate = branchService.GetBranch(id);

            branchToUpdate.Location = branchDTO.Location;
            branchToUpdate.Longitude = branchDTO.Longitude;
            branchToUpdate.Latitude = branchDTO.Latitude;
            branchToUpdate.CityID = branchDTO.CityID;

            branchService.UpdateBranch(branchToUpdate);
            branchService.SaveBranch();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Branch/Prices/5
        [HttpPost]
        [Route("Prices/{id}")]
        [AllowAnonymous]
        public IHttpActionResult UploadPrices(int id)
        {
            HttpPostedFile file = HttpContext.Current.Request.Files[0];
            if (file == null)
            {
                return BadRequest("File not submitted");
            }

            Branch branch = branchService.GetBranch(id);
            branch.PricesFileName = file.FileName;

            using (var binaryReader = new BinaryReader(file.InputStream))
            {
                branch.Prices = binaryReader.ReadBytes(file.ContentLength);
            }

            branchService.UpdateBranch(branch);
            branchService.SaveBranch();

            return Ok();
        }

        // GET: api/Branch/Prices/5
        [HttpGet]
        [Route("Prices/{id}")]
        [AllowAnonymous]
        public void DownloadPrices(int id)
        {
            Branch branch = branchService.GetBranch(id);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(branch.PricesFileName);
            HttpContext.Current.Response.OutputStream.Write(branch.Prices, 0, branch.Prices.Length);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + branch.PricesFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", branch.Prices.Length.ToString());
            HttpContext.Current.Response.End();
        }

        private int getBranchID()
        {
            var identity = (ClaimsPrincipal)Thread.CurrentPrincipal;
            return Convert.ToInt32(identity.Claims.AsQueryable().FirstOrDefault(a => a.Type == "BranchID").Value);
        }
    }
}