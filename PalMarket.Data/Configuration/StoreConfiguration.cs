using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Configuration
{
    public class StoreConfiguration : EntityTypeConfiguration<Store>
    {
        public StoreConfiguration()
        {
            ToTable("Stores");
            Property(g => g.Name).IsRequired().HasMaxLength(100);
            Property(g => g.QRCode).IsRequired().HasMaxLength(100);
            Property(g => g.Image).IsOptional();
            Property(g => g.ImageFileName).IsOptional().HasMaxLength(50);
        }
    }
}
