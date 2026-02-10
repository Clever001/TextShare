using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record DeleteDocumentGrantRequest(
    string DocumentGrantId,
    string CallingUserId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(DocumentGrantId, nameof(DocumentGrantId));
        dtoChecker.CheckForRequiredString(CallingUserId, nameof(CallingUserId));

        return dtoChecker.GetCheckResult();
    }
}