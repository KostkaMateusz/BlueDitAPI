namespace Bluedit.Domain.Entities.ReplyEntities;

public class SubReplay : ReplyBase
{
    public List<SubReplay> ParentReplay = new();
    public Guid ParentReplyId { get; set; }
}