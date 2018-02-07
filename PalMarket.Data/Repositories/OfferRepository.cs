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
    public class OfferRepository : RepositoryBase<Offer>, IOfferRepository
    {
        public OfferRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public override void Update(Offer entity)
        {
            entity.DateUpdated = DateTime.UtcNow;
            base.Update(entity);
        }
    }
}
