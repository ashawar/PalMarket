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
    public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
    {
        public BranchRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

    }
}
