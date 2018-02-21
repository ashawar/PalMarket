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
    [RoutePrefix("api/lookup")]
    [ControllerExceptionFilter]
    public class LookupController : ApiController
    {
        private readonly ILookupService lookupService;

        public LookupController(ILookupService lookupService)
        {
            this.lookupService = lookupService;
        }

        // GET: api/lookups
        public IHttpActionResult GetLookups()
        {
            var lookups = lookupService.GetLookups();
            return Ok(lookups);
        }
    }
}