using Auth.Dto.Shared;
using Auth.Other;

namespace Auth.Dto.Account;

public record UsersPagedFilter(
    string? UserName,
    int PageNumber,
    int PageSize
) : PaginationPageDto(PageNumber, PageSize) {
    public override DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();
        var paginationCheckResult = base.CheckValidity();
        dtoChecker.AppendOtherDtoCheckResult(paginationCheckResult);
        
        dtoChecker.AddErrorIfNotNullEmptyString(UserName, nameof(UserName));

        return dtoChecker.GetCheckResult();
    }
}
