using Auth.Other;

namespace Auth.Dto.Document;

public record UpdateDefaultRoleRequest(
    string DocumentId,
    string DefaultRoleName
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(DocumentId, nameof(DocumentId));
        dtoChecker.CheckForRequiredString(DefaultRoleName, nameof(DefaultRoleName));

        return dtoChecker.GetCheckResult();
    }
}
