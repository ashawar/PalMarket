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
            Mapper.CreateMap<Branch, BranchDTO>()
                 .ForMember(g => g.BranchID, map => map.MapFrom(vm => vm.BranchID))
                 .ForMember(g => g.Location, map => map.MapFrom(vm => vm.Location))
                 .ForMember(g => g.Longitude, map => map.MapFrom(vm => vm.Longitude))
                 .ForMember(g => g.Latitude, map => map.MapFrom(vm => vm.Longitude))
                 .ForMember(g => g.CityID, map => map.MapFrom(vm => vm.CityID))
                 .ForMember(g => g.City, map => map.MapFrom(vm => vm.City.Name))
                 .ForMember(g => g.StoreID, map => map.MapFrom(vm => vm.StoreID))
                 .ForMember(g => g.Store, map => map.MapFrom(vm => vm.Store.Name));
        }
    }
}