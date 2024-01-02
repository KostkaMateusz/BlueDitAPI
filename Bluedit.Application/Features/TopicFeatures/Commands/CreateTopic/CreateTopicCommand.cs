
using Bluedit.Domain.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Bluedit.Application.Features.TopicFeatures.Commands.CreateTopic;

public class CreateTopicCommand : IRequest<Topic>
{
    [MaxLength(20)]
    [MinLength(6)]
    public string TopicName { get => _TopicName; set => _TopicName = value.ToUpper(); }
    [MaxLength(60)]
    [MinLength(6)]
    public required string TopicDescription { get; set; }

    private string _TopicName = string.Empty;
}
