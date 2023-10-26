namespace Bluedit.Entities;

public class Reply : ReplayBase
{
    public Post? ParentPost { get; set; }
    public Guid ParentPostId { get; set; }
}
