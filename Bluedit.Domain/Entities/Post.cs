using Bluedit.Domain.Entities.LikeEntities;
using Bluedit.Domain.Entities.ReplyEntities;

namespace Bluedit.Domain.Entities;

public class Post
{
    public Guid PostId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public Guid ImageGuid { get; set; }
    public User? User { get; set; }
    public Guid? UserId { get; set; }
    public Topic? Topic { get; set; }
    public required string TopicName { get; set; }
    public IEnumerable<Reply> Reply { get; set; }=new List<Reply>();

    public IEnumerable<PostLike> PostLikes { get; set; }  = new List<PostLike>();
}