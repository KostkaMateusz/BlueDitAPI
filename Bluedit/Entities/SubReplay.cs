namespace Bluedit.Entities;

public class SubReplay : ReplayBase
{
    public Guid ParentReplyId { get; set; }

    public List<SubReplay> ParentReplay = new List<SubReplay>();
}