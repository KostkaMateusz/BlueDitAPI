using System.ComponentModel.DataAnnotations;
using Bluedit.Domain.Entities;
using MediatR;

namespace Bluedit.Application.Features.TopicFeatures.Commands.CreateTopic;

public class CreateTopicCommand : IRequest<Topic>
{
    private string _topicName = string.Empty;

    [MaxLength(20)]
    [MinLength(6)]
    public string TopicName
    {
        get => _topicName;
        set => _topicName = value.ToUpper();
    }

    [MaxLength(60)] [MinLength(6)] public required string TopicDescription { get; set; }
}