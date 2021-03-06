﻿using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain.Contracts.Services
{
    // Operations you want to expose
    public interface IBranchService
    {
        Branch GetBranch(int branchID);
        void UpdateBranch(Branch branch);
        void SaveBranch();
    }
}
