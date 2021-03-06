﻿using PalMarket.Data.Comparers;
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

        public List<Device> GetByBranch(int branchId)
        {
            var lst = (from a in DbContext.Devices
                       join b in DbContext.StoreDevices
                       on a.DeviceID equals b.DeviceID
                       join c in DbContext.Stores
                       on b.StoreID equals c.StoreID
                       join d in DbContext.Branches
                       on c.StoreID equals d.StoreID
                       where d.BranchID == branchId
                       select a).ToList().Distinct(new DeviceComparer()).ToList();

            return lst;
        }

        public Device GetByCode(string deviceCode)
        {
            return DbContext.Devices.FirstOrDefault(a => a.DeviceCode == deviceCode);
        }

        public void Subscribe(StoreDevice subscribtion)
        {
            DbContext.StoreDevices.Add(subscribtion);
        }

        public override void Delete(Device device)
        {
            DbContext.StoreDevices.RemoveRange(device.StoreDevices);
            base.Delete(device);
        }
    }
}
