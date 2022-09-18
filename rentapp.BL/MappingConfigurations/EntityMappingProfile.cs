using AutoMapper;
using rentapp.BL.Dtos.Customer;
using rentapp.BL.Entities;
using rentapp.BL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.BL.MappingConfigurations
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
        {
            CreateMap<CustomerDto, Customer>()
                .ForMember(p => p.CustomerAddresses, opt => opt.Ignore())
                .ForMember(p => p.CustomerPaymentFlows, opt => opt.Ignore())
                .ForMember(p => p.CustomerPayments, opt => opt.Ignore())
                .ForMember(p => p.Properties, opt => opt.Ignore())
                .ForMember(p => p.Contracts, opt => opt.Ignore())
                .ForMember(p => p.DateCreated, opt => opt.Ignore())
                  .ForMember(p => p.DateUpdated, opt => opt.Ignore())
                  .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
                  .ForMember(p => p.UpdatedUserId, opt => opt.Ignore());

            CreateMap<Customer, CustomerDto>();

            CreateMap<CustomerAddressDto, CustomerAddress>()
              .ForMember(x => x.PlainAdjacentStreet1, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.AdjacentStreet1)))
              .ForMember(x => x.PlainAdjacentStreet2, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.AdjacentStreet2)))
              .ForMember(x => x.PlainCity, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.City)))
              .ForMember(x => x.PlainCountry, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.Country)))
              .ForMember(x => x.PlainState, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.State)))
              .ForMember(x => x.PlainStreet, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.Street)))
              .ForMember(x => x.PlainZipCode, opt => opt.MapFrom(y => StringProcessor.CleanForSearch(y.ZipCode)))
              .ForMember(p => p.DateCreated, opt => opt.Ignore())
              .ForMember(p => p.DateUpdated, opt => opt.Ignore())
              .ForMember(p => p.CreatedUserId, opt => opt.Ignore())
              .ForMember(p => p.UpdatedUserId, opt => opt.Ignore());



            CreateMap<CustomerAddress, CustomerAddressDto>();
        }
    }
}
