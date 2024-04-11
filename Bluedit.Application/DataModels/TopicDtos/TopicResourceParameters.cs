using Bluedit.Application.Contracts;
using MediatR;

namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicResourceParameters : ResourceParametersBase, IRequest<IPagedList>
{
    public TopicResourceParameters()
    {
        OrderBy = "postCount";
    }

    public string? TopicName { get; set; }
}