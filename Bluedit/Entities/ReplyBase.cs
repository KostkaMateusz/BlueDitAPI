using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Bluedit.Entities;

public abstract class ReplyBase
{
    [Key]
    public Guid ReplyId { get; set; }
    public string? Description { get; set; }
    public User? User { get; set; }
    [AllowNull]
    public Guid? UserId { get; set; }
    public bool IsPostReplay { get; set; }
}