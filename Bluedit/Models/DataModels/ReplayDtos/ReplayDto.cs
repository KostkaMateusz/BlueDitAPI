
namespace Bluedit.Models.DataModels.ReplayDtos;

public record ReplayDto
{    
    public string? Description { get; set; }    
    public Guid UserId { get; set; }
    public Guid ReplayBaseId { get; set; }
}