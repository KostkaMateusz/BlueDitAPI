using AutoMapper;


namespace Bluedit.Models;

public class ReplayMappingProfile : Profile
{
    public ReplayMappingProfile()
    {
        CreateMap<Entities.ReplayBase, DataModels.ReplayDtos.ReplayDto>().ReverseMap();
        CreateMap<Entities.Reply, DataModels.ReplayDtos.ReplayDto>().ReverseMap(); ;
    }
}
