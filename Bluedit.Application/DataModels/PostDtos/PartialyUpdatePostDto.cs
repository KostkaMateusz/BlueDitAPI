namespace Bluedit.Application.DataModels.PostDtos;

public class PartialyUpdatePostDto
{
    public required string Title { get; set; }
    public string? Description { get; set; }
}