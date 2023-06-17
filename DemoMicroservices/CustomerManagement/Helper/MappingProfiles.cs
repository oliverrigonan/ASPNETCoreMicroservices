using System;
using AutoMapper;
using CustomerManagement.Models;

namespace CustomerManagement.Helper
{
	public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMapCustomer();
        }

        public void CreateMapCustomer()
        {
            CreateMap<CustomerModel, CustomerDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            CreateMap<CustomerDTO, CustomerModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}

