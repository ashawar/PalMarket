using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Configuration
{
    public class StoreDeviceConfiguration : EntityTypeConfiguration<StoreDevice>
    {
        public StoreDeviceConfiguration()
        {
            ToTable("StoreDevices");
            Property(g => g.DeviceID).IsRequired();
            Property(g => g.StoreID).IsRequired();
        }
    }
}
