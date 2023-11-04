namespace Bluedit.Entities;

public class SubReplay : ReplyBase
{
    public Guid ParentReplyId { get; set; }

    public List<SubReplay> ParentReplay = new List<SubReplay>();
}