namespace Bluedit.Application.DataModels.TopicDtos;

public class TopicResourceParametersBase : ResourceParametersBase
{
    public TopicResourceParametersBase()
    {
        base.OrderBy  = "postCount";
    }
}
