using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record DocumentGrantDto(
    string DocumentGrantId,
    string DocumentId,
    string RoleName
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(DocumentGrantId, nameof(DocumentGrantId));
        dtoChecker.AddErrorIfNullOrEmptyString(RoleName, nameof(RoleName));

        return dtoChecker.GetCheckResult();
    }
}