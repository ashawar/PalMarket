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
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository branchRepository;
        private readonly IUnitOfWork unitOfWork;

        public BranchService(IBranchRepository branchRepository, IUnitOfWork unitOfWork)
        {
            this.branchRepository = branchRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IBranchService Members
        
        public Branch GetBranch(int branchID)
        {
            return branchRepository.GetById(branchID);
        }
        
        public void UpdateBranch(Branch branch)
        {
            branchRepository.Update(branch);
        }

        public void SaveBranch()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
