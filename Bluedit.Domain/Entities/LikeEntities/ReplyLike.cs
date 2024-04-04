using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Domain.Entities.LikeEntities;

public class ReplyLike : LikeBase
{
    public ReplyBase? Reply { get; set; }
}