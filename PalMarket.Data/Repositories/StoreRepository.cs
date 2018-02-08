using PalMarket.Data.Infrastructure;
using PalMarket.Domain.Contracts.Repositories;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Repositories
{
    public class StoreRepository : RepositoryBase<Store>, IStoreRepository
    {
        public StoreRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public IEnumerable<DetailedStore> GetStores(string deviceCode)
        {
            //this.DbContext.Configuration.AutoDetectChangesEnabled = false;
            //this.DbContext.Configuration.LazyLoadingEnabled = false;
            //this.DbContext.Configuration.ProxyCreationEnabled = false;
            //this.DbContext.Configuration.ValidateOnSaveEnabled = false;

            var lst = (from a in this.DbContext.Stores
                       select new
                       {
                           StoreID = a.StoreID,
                           Name = a.Name,
                           Image = a.Image,
                           QRCode = a.QRCode,
                           DateCreated = a.DateCreated,
                           DateUpdated = a.DateUpdated,
                           IsSubscribed = a.StoreDevices.FirstOrDefault(b => b.Device != null && b.Device.DeviceCode == deviceCode) != null,
                           Branches = a.Branches//.Select(b => new Branch { BranchID = b.BranchID, Location = b.Location, Longitude = b.Longitude, Latitude = b.Latitude, City = b.City })
                       }).ToList()
                       .Select(a => new DetailedStore()
                       {
                           StoreID = a.StoreID,
                           Name = a.Name,
                           Image = a.Image,
                           QRCode = a.QRCode,
                           DateCreated = a.DateCreated,
                           DateUpdated = a.DateUpdated,
                           IsSubscribed = a.IsSubscribed,
                           Branches = a.Branches
                       });

            return lst;
        }

        public Store GetByQR(string storeQR)
        {
            return DbContext.Stores.FirstOrDefault(a => a.QRCode == storeQR);
        }
    }
}
