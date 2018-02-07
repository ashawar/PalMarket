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
    public class DeviceRepository : RepositoryBase<Device>, IDeviceRepository
    {
        public DeviceRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public Device GetByCode(string deviceCode)
        {
            return DbContext.Devices.FirstOrDefault(a => a.DeviceCode == deviceCode);
        }

        public void Subscribe(StoreDevice subscribtion)
        {
            DbContext.StoreDevices.Add(subscribtion);
        }
    }
}
