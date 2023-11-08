namespace Bluedit.Domain.Entities;

public class ReplyLike : LikeBase
{
    public ReplyBase? Reply { get; set; }
}