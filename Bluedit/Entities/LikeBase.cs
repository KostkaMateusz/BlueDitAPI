namespace Bluedit.Entities;

public abstract class LikeBase
{
    public User User { get; set; }
    public Guid UserId { get; set; }

    public Guid ParentId { get; set; }
}


public class ReplyLike : LikeBase
{
    public ReplyBase Reply { get; set; }
}


public class PostLike : LikeBase
{
    public Post Post { get; set; }
}
