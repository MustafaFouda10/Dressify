using AutoMapper;
using Dressify.API.DTOs;
using Dressify.API.Models;

namespace Dressify.API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Dress, DressDto>()
                .ForMember(dest => dest.ImageUrl,
                           opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImagePath) ? null : "/" + src.ImagePath.Replace("\\", "/")));
            CreateMap<CreateDressDto, Dress>();

            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.DressName, opt => opt.MapFrom(src => src.Dress != null ? src.Dress.Name : string.Empty));
            CreateMap<CreateReservationDto, Reservation>();
        }
    }
}
