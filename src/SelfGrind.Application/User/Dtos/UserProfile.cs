using AutoMapper;
using SelfGrind.Domain.Entities;

namespace SelfGrind.Application.User.Dtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Domain.Entities.User, UserStatsDto>()
            .ForMember(d => d.AttributeStats, opt => opt.MapFrom(src => src.Stats));

        CreateMap<UserStat, AttributeStatDto>();
    }
}
