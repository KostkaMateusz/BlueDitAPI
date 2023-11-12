namespace Bluedit.Models.DataModels.UserDtos;

public class UserNameIdDto
{
    public Guid UserId { get; set; }
    public required string Name { get; set; }
}