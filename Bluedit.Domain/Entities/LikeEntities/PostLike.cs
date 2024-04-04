namespace Bluedit.Domain.Entities.LikeEntities;

public class PostLike : LikeBase
{
    public Post? Post { get; set; }
}