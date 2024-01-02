using AutoMapper;
using Bluedit.Application.Features.TopicFeatures.Queries.GetTopic;
using Bluedit.Domain.Entities;

namespace Bluedit.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Topic, GetTopicQueryResponse>();
    }
}
