using AutoMapper;
using MTPCoverageIssue.Model;

namespace MTPCoverageIssue.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CountGroupByVersion, VersionCountDto>()
            .ForMember(dst => dst.Version,
                opt => opt.MapFrom(src => src.Version))
            .ForMember(dst => dst.Count,
                opt => opt.MapFrom(src => src.Count))
            .ForMember(dst => dst.Memberships,
                opt => opt.MapFrom(src => src.Memberships));
        CreateMap<CountGroupByVersion.Membership, VersionCountDto.MembershipFlatDto>()
            .ForMember(dst => dst.Version,
                opt => opt.MapFrom(src => src.Version))
            .ForMember(dst => dst.MemberName,
                opt => opt.MapFrom(src => src.MemberName));
    }
}
