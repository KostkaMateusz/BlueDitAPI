using System.ComponentModel.DataAnnotations;

namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicCreatedDto
{
    [MaxLength(20)]
    [MinLength(6)]
    public string TopicName { get => _topicName; set => _topicName = value.ToUpper(); }
    [MaxLength(60)]
    [MinLength(6)]
    public required string TopicDescription { get; set; }

    private string _topicName = string.Empty;
}