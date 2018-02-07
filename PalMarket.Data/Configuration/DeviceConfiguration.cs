using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Configuration
{
    public class DeviceConfiguration : EntityTypeConfiguration<Device>
    {
        public DeviceConfiguration()
        {
            ToTable("Devices");
            Property(g => g.DeviceCode).IsRequired().HasMaxLength(100);
            Property(g => g.RegisterID).IsOptional().HasMaxLength(100);
        }
    }
}
