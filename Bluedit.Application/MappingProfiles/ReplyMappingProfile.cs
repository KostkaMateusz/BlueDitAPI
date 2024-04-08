using AutoMapper;
using Bluedit.Application.DataModels.ReplayDtos;
using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Application.MappingProfiles;

public class ReplyMappingProfile : Profile
{
    public ReplyMappingProfile()
    {
        CreateMap<ReplyBase, ReplyDto>().ReverseMap();
        CreateMap<Reply, ReplyDto>().ReverseMap();
        CreateMap<SubReplay, CreateReplyDto>().ReverseMap();
    }
}