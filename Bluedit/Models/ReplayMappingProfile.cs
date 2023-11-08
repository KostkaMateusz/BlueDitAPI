using AutoMapper;
using Bluedit.Domain.Entities;

namespace Bluedit.Models;

public class ReplayMappingProfile : Profile
{
    public ReplayMappingProfile()
    {
        CreateMap<ReplyBase, DataModels.ReplayDtos.ReplayDto>().ReverseMap();
        CreateMap<Reply, DataModels.ReplayDtos.ReplayDto>().ReverseMap();
        CreateMap<SubReplay, DataModels.ReplayDtos.CreateReplayDto>().ReverseMap();        
    }
}