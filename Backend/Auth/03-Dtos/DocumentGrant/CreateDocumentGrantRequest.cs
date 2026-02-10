using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record CreateDocumentGrantRequest(
    string DocumentId,
    string RoleName,
    string CallingUserId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(DocumentId, nameof(DocumentId));
        dtoChecker.AddErrorIfNullOrEmptyString(RoleName, nameof(RoleName));
        dtoChecker.AddErrorIfNullOrEmptyString(CallingUserId, nameof(CallingUserId));

        return dtoChecker.GetCheckResult();
    }
}
