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
    public class LookupService : ILookupService
    {
        private readonly ILookupRepository lookupRepository;
        private readonly IUnitOfWork unitOfWork;

        public LookupService(ILookupRepository lookupRepository, IUnitOfWork unitOfWork)
        {
            this.lookupRepository = lookupRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ILookupService Members

        public Dictionary<string, List<LookupItem>> GetLookups()
        {
            return lookupRepository.GetLookups();
        }

        #endregion
    }
}
