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
            var lst = (from a in this.DbContext.Stores
                       select new
                       {
                           StoreID = a.StoreID,
                           Name = a.Name,
                           ImageUrl = "",
                           QRCode = a.QRCode,
                           DateCreated = a.DateCreated,
                           DateUpdated = a.DateUpdated,
                           IsSubscribed = a.StoreDevices.FirstOrDefault(b => b.Device != null && b.Device.DeviceCode == deviceCode) != null
                       }).ToList()
                       .Select(a => new DetailedStore()
                       {
                           StoreID = a.StoreID,
                           Name = a.Name,
                           ImageUrl = a.ImageUrl,
                           QRCode = a.QRCode,
                           DateCreated = a.DateCreated,
                           DateUpdated = a.DateUpdated,
                           IsSubscribed = a.IsSubscribed
                       });

            return lst;
        }

        public Store GetByQR(string storeQR)
        {
            return DbContext.Stores.FirstOrDefault(a => a.QRCode == storeQR);
        }
    }
}
