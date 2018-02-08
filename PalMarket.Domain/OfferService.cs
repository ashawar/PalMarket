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
    public class OfferService : IOfferService
    {
        private readonly IOfferRepository offerRepository;
        private readonly IUnitOfWork unitOfWork;

        public OfferService(IOfferRepository offerRepository, IUnitOfWork unitOfWork)
        {
            this.offerRepository = offerRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IOfferService Members

        public IEnumerable<Offer> GetBranchOffers(int branchID)
        {
            return offerRepository.GetMany(a => a.BranchID == branchID);
        }

        public Offer GetOffer(int offerID)
        {
            return offerRepository.GetById(offerID);
        }

        public void AddOffer(Offer offer)
        {
            offerRepository.Add(offer);
        }

        public void UpdateOffer(Offer offer)
        {
            offerRepository.Update(offer);
        }

        public void DeleteOffer(Offer offer)
        {
            offerRepository.Delete(offer);
        }

        public void SaveOffer()
        {
            unitOfWork.Commit();
        }

        #endregion
    }
}
