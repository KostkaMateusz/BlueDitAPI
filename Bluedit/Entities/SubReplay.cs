namespace Bluedit.Entities;

public class SubReplay : ReplayBase
{
    public Guid ParentReplyId { get; set; }

    public IEnumerable<SubReplay> ParentReplay = new List<SubReplay>();
}