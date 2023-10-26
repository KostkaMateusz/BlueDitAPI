namespace Bluedit.Models.DataModels.UserDtos;

public record UserNameIdDto
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
}