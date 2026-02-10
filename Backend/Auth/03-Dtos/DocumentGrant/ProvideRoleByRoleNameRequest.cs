using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record ProvideRoleByRoleNameRequest(
    string DocumentId,
    string RoleName,
    string UserIdToProvideRole,
    string CallingUserId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(DocumentId, nameof(DocumentId));
        dtoChecker.AddErrorIfNullOrEmptyString(RoleName, nameof(RoleName));
        dtoChecker.AddErrorIfNullOrEmptyString(UserIdToProvideRole, nameof(UserIdToProvideRole));
        dtoChecker.AddErrorIfNullOrEmptyString(CallingUserId, nameof(CallingUserId));

        return dtoChecker.GetCheckResult();
    }
}
