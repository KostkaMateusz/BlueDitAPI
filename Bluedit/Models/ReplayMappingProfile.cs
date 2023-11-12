using AutoMapper;
using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Models;

public class ReplayMappingProfile : Profile
{
    public ReplayMappingProfile()
    {
        CreateMap<ReplyBase, DataModels.ReplayDtos.ReplyDto>().ReverseMap();
        CreateMap<Reply, DataModels.ReplayDtos.ReplyDto>().ReverseMap();
        CreateMap<SubReplay, DataModels.ReplayDtos.CreateReplayDto>().ReverseMap();        
    }
}