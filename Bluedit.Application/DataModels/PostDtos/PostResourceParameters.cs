namespace Bluedit.Application.DataModels.PostDtos;

public class PostResourceParameters : ResourceParametersBase
{
    public PostResourceParameters()
    {
        OrderBy = "CreationDate";
    }

    public string? UserName { get; set; }
    public string? TopicName { get; set; }
}