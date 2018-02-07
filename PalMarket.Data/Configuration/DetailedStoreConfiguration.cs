using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Configuration
{
    public class DetailedStoreConfiguration : EntityTypeConfiguration<DetailedStore>
    {
        public DetailedStoreConfiguration()
        {
            Ignore(a => a.IsSubscribed);
        }
    }
}
