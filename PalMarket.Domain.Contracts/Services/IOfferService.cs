using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Domain.Contracts.Services
{
    // Operations you want to expose
    public interface IOfferService
    {
        IEnumerable<Offer> GetBranchOffers(int branchID);
        Offer GetOffer(int offerID);
        void AddOffer(Offer offer);
        void UpdateOffer(Offer offer);
        void DeleteOffer(Offer offer);
        void SaveOffer();
    }
}
