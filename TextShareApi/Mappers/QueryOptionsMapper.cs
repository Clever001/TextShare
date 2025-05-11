using System.Reflection;
using TextShareApi.Dtos.QueryOptions;

namespace TextShareApi.Mappers;

public static class QueryOptionsMapper {
    public static bool IsValid(this SortDto dto, Type type) {
        string searchType = dto.SortBy;
        var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        return props.Any(propInfo =>
            propInfo.Name.Equals(searchType, StringComparison.InvariantCultureIgnoreCase));
    }
}