using System;
using AutoMapper;
using ProductManagement.DTOs;
using ProductManagement.Models;

namespace ProductManagement.Helper
{
	public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMapCustomer();
        }

        public void CreateMapCustomer()
        {
            CreateMap<ProductModel, ProductDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<ProductDTO, ProductModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }
    }
}

