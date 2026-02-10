using Auth.Dto.Shared;
using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record DocumentGrantsPagedFilter(
    string? DocumentId,
    string? RoleName,
    string CallingUserId,
    int PageNumber,
    int PageSize
) : PaginationPageDto(PageNumber, PageSize), ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        throw new NotImplementedException();
    }
}