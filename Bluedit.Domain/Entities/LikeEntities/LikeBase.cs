namespace Bluedit.Domain.Entities.LikeEntities;

public abstract class LikeBase
{
    public User? User { get; set; }
    public Guid UserId { get; set; }
    public Guid ParentId { get; set; }
}