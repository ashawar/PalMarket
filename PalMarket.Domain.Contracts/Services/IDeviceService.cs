using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain.Contracts.Services
{
    // Operations you want to expose
    public interface IDeviceService
    {
        List<Device> GetByBranch(int branchId);
        Device GetByCode(string deviceCode);
        void AddDevice(Device device);
        void DeleteDevice(Device device);
        void Subscribe(StoreDevice subscribtion);
        void SaveDevice();
    }
}
