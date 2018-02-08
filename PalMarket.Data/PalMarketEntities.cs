using PalMarket.Data.Configuration;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data
{
    public class PalMarketEntities : DbContext
    {
        public PalMarketEntities() : base("PalMarketEntities") { }

        public DbSet<Offer> Offers { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<StoreDevice> StoreDevices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Branch> Branches { get; set; }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OfferConfiguration());
            modelBuilder.Configurations.Add(new StoreConfiguration());
            modelBuilder.Configurations.Add(new DeviceConfiguration());
            modelBuilder.Configurations.Add(new StoreDeviceConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
        }
    }
}
