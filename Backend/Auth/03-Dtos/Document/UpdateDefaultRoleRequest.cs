using Auth.Other;

namespace Auth.Dto.Document;

public record UpdateDefaultRoleRequest(
    string DocumentId,
    string DefaultRoleName
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(DocumentId, nameof(DocumentId));
        dtoChecker.AddErrorIfNullOrEmptyString(DefaultRoleName, nameof(DefaultRoleName));

        return dtoChecker.GetCheckResult();
    }
}
