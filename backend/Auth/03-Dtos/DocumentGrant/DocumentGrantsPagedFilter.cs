using Auth.Dto.Shared;
using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record DocumentGrantsPagedFilter(
    string? DocumentId,
    string? RoleName,
    string CallingUserId,
    int PageNumber,
    int PageSize
) : PaginationPageDto(PageNumber, PageSize) {
    public override DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();
        var baseCheckResult = base.CheckValidity();
        dtoChecker.AppendOtherDtoCheckResult(baseCheckResult);
        
        dtoChecker.AddErrorIfNotNullEmptyString(DocumentId, nameof(DocumentId));
        dtoChecker.AddErrorIfNotNullEmptyString(RoleName, nameof(RoleName));
        dtoChecker.AddErrorIfNullOrEmptyString(CallingUserId, nameof(CallingUserId));

        return dtoChecker.GetCheckResult();
    }
}