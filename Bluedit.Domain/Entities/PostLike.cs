namespace Bluedit.Domain.Entities;

public class PostLike : LikeBase
{
    public Post? Post { get; set; }
}