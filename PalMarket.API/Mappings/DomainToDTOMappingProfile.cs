using AutoMapper;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PalMarket.API.DTOs;

namespace PalMarket.API.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToDTOMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Offer, OfferDTO>();
            Mapper.CreateMap<Store, StoreDTO>();
            Mapper.CreateMap<DetailedStore, StoreDTO>();
            Mapper.CreateMap<Device, DeviceDTO>();
        }
    }
}