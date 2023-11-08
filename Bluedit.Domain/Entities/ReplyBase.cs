namespace Bluedit.Domain.Entities;

public abstract class ReplyBase
{
    public Guid ReplyId { get; set; }
    public string? Description { get; set; }
    public User? User { get; set; }
    public Guid? UserId { get; set; }
    public bool IsPostReplay { get; set; }
    public IEnumerable<ReplyLike> ReplyLikes { get; set; } = new List<ReplyLike>();
}