using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.TopicDtos;

public class TopicCreateDto
{
    [MaxLength(20)]
    [MinLength(6)]
    public string TopicName { get=> _TopicName; set => _TopicName = value.ToUpper(); }
    [MaxLength(60)]
    [MinLength(6)]
    public string TopicDescription { get; set; }

    private string _TopicName;

}
