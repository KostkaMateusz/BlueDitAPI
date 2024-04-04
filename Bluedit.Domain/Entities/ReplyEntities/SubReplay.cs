namespace Bluedit.Domain.Entities.ReplyEntities;

public class SubReplay : ReplyBase
{
    public Guid ParentReplyId { get; set; }

    public List<SubReplay> ParentReplay = new List<SubReplay>();
}