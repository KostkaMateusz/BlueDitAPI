using Bluedit.Application.Contracts;
using Bluedit.Application.Features.TopicFeatures.Queries.GetTopicsQuery;
using Bluedit.Domain.Entities;
using Bluedit.Services.Repositories.TopicRepo;
using MediatR;

namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicResourceParameters : ResourceParametersBase, IRequest<IPagedList<Topic>>
{
    public TopicResourceParameters()
    {
        base.OrderBy  = "postCount";
    }
}
