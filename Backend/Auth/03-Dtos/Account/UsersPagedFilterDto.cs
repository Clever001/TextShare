using Auth.Dto.Shared;

namespace Auth.Dto.Account;

public record UsersPagedFilterDto(
    string? UserName,
    int PageNumber,
    int PageSize
) : PaginationPageDto(PageNumber, PageSize);
