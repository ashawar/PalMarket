using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Configuration
{
    public class OfferConfiguration : EntityTypeConfiguration<Offer>
    {
        public OfferConfiguration()
        {
            ToTable("Offers");
            Property(g => g.Name).IsRequired().HasMaxLength(100);
            Property(g => g.DateStart).IsRequired();
            Property(g => g.OldPrice).HasPrecision(8, 2);
            Property(g => g.NewPrice).HasPrecision(8, 2);
            Property(g => g.ImageFileName).IsOptional();
            Property(g => g.ImageFileName).IsOptional().HasMaxLength(50);
        }
    }
}
