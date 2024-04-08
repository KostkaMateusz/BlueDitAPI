
namespace Bluedit.Application.DataModels.ReplayDtos;

public class ReplyDto
{
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public Guid ReplyId { get; set; }
    public DateTime CreationDate { get; set; }
}