using AutoMapper;
using PalMarket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PalMarket.API.DTOs;

namespace PalMarket.API.Mappings
{
    public class DTOToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DTOToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<OfferDTO, Offer>();
            Mapper.CreateMap<DeviceDTO, Device>();
            Mapper.CreateMap<SubscribeDTO, StoreDevice>();
            //Mapper.CreateMap<GadgetFormViewModel, Gadget>()
            //    .ForMember(g => g.Name, map => map.MapFrom(vm => vm.GadgetTitle))
            //    .ForMember(g => g.Description, map => map.MapFrom(vm => vm.GadgetDescription))
            //    .ForMember(g => g.Price, map => map.MapFrom(vm => vm.GadgetPrice))
            //    .ForMember(g => g.Image, map => map.MapFrom(vm => vm.File.FileName))
            //    .ForMember(g => g.CategoryID, map => map.MapFrom(vm => vm.GadgetCategory));
        }
    }
}