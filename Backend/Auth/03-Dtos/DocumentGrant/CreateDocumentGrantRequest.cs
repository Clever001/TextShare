using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record CreateDocumentGrantRequest(
    string DocumentId,
    string RoleName,
    string CallingUserId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(DocumentId, nameof(DocumentId));
        dtoChecker.CheckForRequiredString(RoleName, nameof(RoleName));
        dtoChecker.CheckForRequiredString(CallingUserId, nameof(CallingUserId));

        return dtoChecker.GetCheckResult();
    }
}
