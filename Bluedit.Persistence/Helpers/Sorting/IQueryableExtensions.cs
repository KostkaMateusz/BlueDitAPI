using System.Linq.Dynamic.Core;
using System.Text;
using Microsoft.Extensions.Primitives;

namespace Bluedit.Persistence.Helpers.Sorting;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        ArgumentNullException.ThrowIfNull(source);

        ArgumentNullException.ThrowIfNull(mappingDictionary);

        if (string.IsNullOrWhiteSpace(orderBy))
            return source;

        var orderByStringNuBuilder = new StringBuilder();

        // the orderBy string is separated by ",", so we split it.
        var orderByAfterSplit = orderBy.Split(',');

        // apply each order by clause  
        foreach (var orderByClause in orderByAfterSplit)
        {
            // trim the orderBy clause, as it might contain leading
            // or trailing spaces. Can't trim the var in foreach,
            // so use another var.
            var trimmedOrderByClause = orderByClause.Trim();

            // if the sort option ends with with " desc", we order
            // descending, otherwise ascending
            var orderDescending = trimmedOrderByClause.EndsWith(" desc");

            // remove " asc" or " desc" from the orderBy clause, so we 
            // get the property name to look for in the mapping dictionary
            var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);
            
            var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

            // find the matching property
            if (!mappingDictionary.TryGetValue(propertyName, out PropertyMappingValue? value))
                throw new ArgumentException($"Key mapping for {propertyName} is missing");

            // get the PropertyMappingValue
            var propertyMappingValue = value;

            if (propertyMappingValue is null)
                throw new ArgumentNullException(nameof(propertyMappingValue));

            // revert sort order if necessary
            if (propertyMappingValue.Revert)
                orderDescending = !orderDescending;
            
            // Run through the property names 
            foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
            {
                orderByStringNuBuilder.Append(orderByStringNuBuilder.Length == 0 ? string.Empty : ", ");
                orderByStringNuBuilder.Append(destinationProperty);
                orderByStringNuBuilder.Append(orderDescending ? " descending" : " ascending");
            }
        }
        return source.OrderBy(orderByStringNuBuilder.ToString());
    }
}