using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain.Contracts.Services
{
    // Operations you want to expose
    public interface IStoreService
    {
        IEnumerable<DetailedStore> GetStores(string deviceCode);
        Store GetStore(int storeID);
        Store GetByQR(string storeQR);
        void UpdateStore(Store store);
        void SaveStore();
    }
}
