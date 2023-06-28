using System.Diagnostics.CodeAnalysis;

namespace Bluedit.Entities;

public class Post
{
    public Guid PostId { get; set; }
    [AllowNull]
    public Guid? ParentPostId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public Guid ImageGuid { get; set; }
    public User User { get; set; }
    [AllowNull]
    public Guid? UserId { get; set; }
}
