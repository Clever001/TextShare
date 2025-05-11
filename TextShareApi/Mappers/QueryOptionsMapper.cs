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

    public static PaginatedResponseDto<T2> Convert<T1, T2>(this PaginatedResponseDto<T1> dto, Func<T1, T2> mapper) {
        return new PaginatedResponseDto<T2> {
            Items = dto.Items.Select(mapper).ToList(),
            CurrentPage = dto.CurrentPage,
            PageSize = dto.PageSize,
            TotalItems = dto.TotalItems,
            TotalPages = dto.TotalPages
        };
    }

    public static PaginatedResponseDto<T> ToPaginatedResponse<T>(this List<T> list, PaginationDto pagination, int count) {
        return new PaginatedResponseDto<T> {
            Items = list,
            CurrentPage = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalItems = count,
            TotalPages = (int)Math.Ceiling(count / (double)pagination.PageSize)
        };
    } 
}