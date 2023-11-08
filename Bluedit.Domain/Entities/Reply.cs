namespace Bluedit.Domain.Entities;

public class Reply : ReplyBase
{
    public Post? ParentPost { get; set; }
    public Guid ParentPostId { get; set; }
}