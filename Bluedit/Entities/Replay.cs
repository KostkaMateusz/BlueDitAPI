using System.Diagnostics.CodeAnalysis;

namespace Bluedit.Entities;

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

    public List<ReplyToReply> ParentReplay = new List<ReplyToReply>();
}
