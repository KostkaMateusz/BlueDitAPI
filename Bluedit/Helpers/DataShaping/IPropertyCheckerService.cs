namespace Bluedit.Helpers.DataShaping;

public interface IPropertyCheckerService
{
    bool TypeHasProperties<T>(string? fields);
}