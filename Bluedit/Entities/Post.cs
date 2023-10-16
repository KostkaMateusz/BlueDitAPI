using System.Diagnostics.CodeAnalysis;

namespace Bluedit.Entities;

public class Post
{
    public Guid PostId { get; set; }
    [AllowNull]
    public Guid? ParentPostId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public Guid ImageGuid { get; set; }
    public User User { get; set; }
    [AllowNull]
    public Guid? UserId { get; set; }
    public Topic Topic { get; set; }
    public required string TopicName { get; set; }
    public List<Reply?> Reply { get; set; }=new List<Reply>();
}


public abstract class ReplayBase
{
    public Guid ReplayBaseId { get; set; }
    public string? Description { get; set; }
    public User User { get; set; }
    [AllowNull]
    public Guid? UserId { get; set; }
    public bool IsPostReplay { get; set; } 

}

public class Reply : ReplayBase
{
    public Post ParentPost { get; set; }
    public Guid ParentPostId { get; set; }
}

public class ReplyToReply : ReplayBase
{   
    public Guid ParentReplyId { get; set; }

    public List<ReplyToReply> ParentReplay=new List<ReplyToReply>();
}

