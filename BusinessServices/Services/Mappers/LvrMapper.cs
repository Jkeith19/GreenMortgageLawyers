using AutoMapper;
using Entity.DTOs;
using Entity.Entity.Models;

namespace BusinessServices.Services.Mappers
{
    public class LvrMapper : ILvrMapper
    {
        public IMapper LvrConfiguration()
        {
            return new MapperConfiguration(map =>
            {
                map.CreateMap<LVR, LVRDto>()
                .ForMember(dest => dest.LVR, opt => opt.MapFrom(src => src.LoanValuationRatio)).ReverseMap();
            }).CreateMapper();
        }
    }
}
