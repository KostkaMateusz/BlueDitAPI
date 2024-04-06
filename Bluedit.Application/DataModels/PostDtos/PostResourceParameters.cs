namespace Bluedit.Application.DataModels.PostDtos;

public class PostResourceParameters : ResourceParametersBase 
{
    public string? UserName { get; set; }
    public string? TopicName { get; set; }
    public PostResourceParameters()
    {
        OrderBy = "CreationDate";
    }
}
