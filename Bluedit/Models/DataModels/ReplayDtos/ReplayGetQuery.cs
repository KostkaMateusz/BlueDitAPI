using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bluedit.Models.DataModels.ReplayDtos;

public record ReplayGetQuery
{
    [BindRequired]
    public Guid QueryId { get; set; }
}
