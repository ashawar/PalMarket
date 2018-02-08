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

            City city = AddCities();
            Store store = AddStore();
            Branch branch = AddBranch(store, city);
            User user = AddUser(branch);

            context.Commit();
        }

        private City AddCities()
        {
            IEnumerable<City> cities = dbContext.Cities.AddRange(new List<City>
            {
                new City { Name = "أريحا" },
                new City { Name = "الخليل" },
                new City { Name = "القدس" },
                new City { Name = "بيت لحم" },
                new City { Name = "جنين" },
                new City { Name = "رام الله والبيرة" },
                new City { Name = "سلفيت" },
                new City { Name = "طوباس" },
                new City { Name = "طولكرم" },
                new City { Name = "قلقيلية" },
                new City { Name = "نابلس" }
            });

            dbContext.Commit();
            return cities.FirstOrDefault(a => a.Name == "رام الله والبيرة");
        }

        private Store AddStore()
        {
            return dbContext.Stores.Add(new Store()
            {
                Name = "Store1",
                QRCode = "store1",
                DateCreated = DateTime.UtcNow
            });
        }

        private Branch AddBranch(Store store, City city)
        {
            return dbContext.Branches.Add(new Branch()
            {
                Location = "البالوع",
                StoreID = store.StoreID,
                CityID = city.CityID
            });
        }

        private User AddUser(Branch branch)
        {
            return dbContext.Users.Add(new User()
            {
                FirstName = "Admin",
                LastName = "Admin",
                Username = "store1",
                PasswordHash = SecurityHelper.HashPassword("store1"),
                BranchID = branch.BranchID,
                DateCreated = DateTime.UtcNow
            });
        }
    }
}
