using Bluedit.Application.Contracts;
using Bluedit.Domain.Entities;
using MediatR;

namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicResourceParameters : ResourceParametersBase, IRequest<IPagedList<Topic>>
{
    public string? TopicName { get; set; }
    public TopicResourceParameters()
    {
        base.OrderBy  = "postCount";
    }
}
