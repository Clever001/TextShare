using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record DocumentGrantDto(
    string DocumentGrantId,
    string DocumentId,
    string RoleName
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(DocumentGrantId, nameof(DocumentGrantId));
        dtoChecker.CheckForRequiredString(RoleName, nameof(RoleName));

        return dtoChecker.GetCheckResult();
    }
}