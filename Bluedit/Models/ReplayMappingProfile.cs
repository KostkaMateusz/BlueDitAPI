using AutoMapper;
using Bluedit.Application.DataModels.ReplayDtos;
using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Models;

public class ReplayMappingProfile : Profile
{
    public ReplayMappingProfile()
    {
        CreateMap<ReplyBase, ReplyDto>().ReverseMap();
        CreateMap<Reply, ReplyDto>().ReverseMap();
        CreateMap<SubReplay, CreateReplyDto>().ReverseMap();
    }
}