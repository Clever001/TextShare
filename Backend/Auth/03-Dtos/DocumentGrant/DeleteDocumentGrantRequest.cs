using Auth.Other;

namespace Auth.Dto.DocumentGrant;

public record DeleteDocumentGrantRequest(
    string DocumentGrantId,
    string CallingUserId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(DocumentGrantId, nameof(DocumentGrantId));
        dtoChecker.AddErrorIfNullOrEmptyString(CallingUserId, nameof(CallingUserId));

        return dtoChecker.GetCheckResult();
    }
}