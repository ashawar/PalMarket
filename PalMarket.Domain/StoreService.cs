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
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository storeRepository;
        private readonly IUnitOfWork unitOfWork;

        public StoreService(IStoreRepository storeRepository, IUnitOfWork unitOfWork)
        {
            this.storeRepository = storeRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IStoreService Members

        public IEnumerable<DetailedStore> GetStores(string deviceCode)
        {
            return storeRepository.GetStores(deviceCode);
        }

        public Store GetStore(int storeID)
        {
            return storeRepository.GetById(storeID);
        }

        public Store GetByQR(string storeQR)
        {
            return storeRepository.GetByQR(storeQR);
        }

        public void UpdateStore(Store store)
        {
            storeRepository.Update(store);
        }

        public void SaveStore()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
