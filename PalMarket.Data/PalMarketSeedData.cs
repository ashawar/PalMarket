using PalMarket.Common.Helpers;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data
{
    public class PalMarketSeedData : DropCreateDatabaseIfModelChanges<PalMarketEntities>
    {
        private PalMarketEntities dbContext { get; set; }

        protected override void Seed(PalMarketEntities context)
        {
            dbContext = context;

            Store store = AddStore();
            User user = AddUser(store);

            context.Commit();
        }

        private Store AddStore()
        {
            return dbContext.Stores.Add(new Store()
            {
                Name = "Test Store",
                QRCode = "12345",
                DateCreated = DateTime.UtcNow
            });
        }

        private User AddUser(Store store)
        {
            return dbContext.Users.Add(new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Username = "admin",
                PasswordHash = SecurityHelper.HashPassword("admin"),
                StoreID = store.StoreID,
                DateCreated = DateTime.UtcNow
            });
        }
    }
}
