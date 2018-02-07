using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        PalMarketEntities dbContext;

        public PalMarketEntities Init()
        {
            return dbContext ?? (dbContext = new PalMarketEntities());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
