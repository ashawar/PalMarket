using PalMarket.Common.Helpers;
using PalMarket.Data.Infrastructure;
using PalMarket.Data.Repositories;
using PalMarket.Domain;
using PalMarket.Domain.Contracts;
using PalMarket.Domain.Contracts.Repositories;
using PalMarket.Domain.Contracts.Services;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace PalMarket.API
{
    /// <summary>
    /// Summary description for File
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class File : System.Web.Services.WebService
    {
        static IDbFactory dbFactory = new DbFactory();
        static IStoreRepository storeRepository = new StoreRepository(dbFactory);
        static IOfferRepository offerRepository = new OfferRepository(dbFactory);
        static IBranchRepository branchRepository = new BranchRepository(dbFactory);
        static IUnitOfWork unitOfWork = new UnitOfWork(dbFactory);
        IStoreService storeService = new StoreService(storeRepository, unitOfWork);
        IOfferService offerService = new OfferService(offerRepository, unitOfWork);
        IBranchService branchService = new BranchService(branchRepository, unitOfWork);

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void OfferImage(int id)
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
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void StoreImage(int id)
        {
            // Get the store record
            Store store = storeService.GetStore(id);
            string path = ConfigHelper.GetDocumentsFolderPath() + "Stores/Images/" + id;

            // Write the file directly to the HTTP content output stream.
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(store.ImageFileName);
            HttpContext.Current.Response.WriteFile(path);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + store.ImageFileName);
            HttpContext.Current.Response.AddHeader("FileName", store.ImageFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", store.ImageFileSize.ToString());
            HttpContext.Current.Response.End();
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void BranchPrices(int id)
        {
            Branch branch = branchService.GetBranch(id);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = MimeMapping.GetMimeMapping(branch.PricesFileName);
            HttpContext.Current.Response.OutputStream.Write(branch.Prices, 0, branch.Prices.Length);
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + branch.PricesFileName);
            HttpContext.Current.Response.AddHeader("Content-Length", branch.Prices.Length.ToString());
            HttpContext.Current.Response.End();
        }
    }
}
