
namespace Bluedit.Entities;

public class User
{    
    public Guid UserId { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public string? PasswordHash { get; set; }
    public DateTime CreationTime { get; set; }
    public List<Post> Posts { get; set; }
    public string role { get; set; } = "StandartUser";
}
