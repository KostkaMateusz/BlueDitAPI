namespace Bluedit.Application.DataModels.PostDtos;

public class PostInfoDto
{
    public Guid PostId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public Guid ImageGuid { get; set; }
    public string? UserName { get; set; }
    public Guid? UserId { get; set; }
    public string? TopicName { get; set; }
    public string? ImageContentLink { get; set; }
    public int NumberOfReplies { get; set; } = 0;
    public int NumberOfLikes { get; set; } =0;
}