namespace Bluedit.Models.DataModels.PostDtos;

public record PostInfoDto
{
    public Guid PostId { get; set; }   
    public Guid? ParentPostId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public Guid ImageGuid { get; set; }
    public string UserName { get; set; }   
    public Guid? UserId { get; set; }
    public  string TopicName { get; set; }
    public string ImageContentLink { get; set; }
    public int UpVotes { get; set; } = 0;
    public int DownVotes { get; set; } = 0;
    public int NumberOfReplies { get; set; } = 0;
}
