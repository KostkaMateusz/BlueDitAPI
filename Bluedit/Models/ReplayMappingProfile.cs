using AutoMapper;


namespace Bluedit.Models;

public class ReplayMappingProfile : Profile
{
    public ReplayMappingProfile()
    {
        CreateMap<Entities.ReplyBase, DataModels.ReplayDtos.ReplayDto>().ReverseMap();
        CreateMap<Entities.Reply, DataModels.ReplayDtos.ReplayDto>().ReverseMap();
        CreateMap<Entities.SubReplay, DataModels.ReplayDtos.CreateReplayDto>().ReverseMap();        
    }
}
