using System.ComponentModel.DataAnnotations;

namespace Bluedit.Models.DataModels.ReplayDtos;

public sealed record CreateSingleReplyDto : CreateReplayDto
{
    [Required]
    public Guid ParentId { get; set; }
}