using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Configuration
{
    public class BranchConfiguration : EntityTypeConfiguration<Branch>
    {
        public BranchConfiguration()
        {
            ToTable("Branches");
            Property(g => g.Location).IsRequired().HasMaxLength(100);
            Property(g => g.CityID).IsRequired();
            Property(g => g.StoreID).IsRequired();
        }
    }
}
