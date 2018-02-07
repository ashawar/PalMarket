using PalMarket.Domain.Contracts;
using PalMarket.Domain.Contracts.Repositories;
using PalMarket.Domain.Contracts.Services;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRepository deviceRepository;
        private readonly IUnitOfWork unitOfWork;

        public DeviceService(IDeviceRepository deviceRepository, IUnitOfWork unitOfWork)
        {
            this.deviceRepository = deviceRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IDeviceService Members

        public Device GetByCode(string deviceCode)
        {
            return deviceRepository.GetByCode(deviceCode);
        }

        public void AddDevice(Device device)
        {
            deviceRepository.Add(device);
        }

        public void Subscribe(StoreDevice subscribtion)
        {
            deviceRepository.Subscribe(subscribtion);
        }

        public void SaveDevice()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
