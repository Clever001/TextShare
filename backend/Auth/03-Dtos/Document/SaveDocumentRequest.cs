using Auth.Other;

namespace Auth.Dto.Document;

public record SaveDocumentRequest(
    string DocumentId,
    string CreatorId,
    string DefaultRoleName
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(DocumentId, nameof(DocumentId));
        dtoChecker.AddErrorIfNullOrEmptyString(CreatorId, nameof(CreatorId));
        dtoChecker.AddErrorIfNullOrEmptyString(DefaultRoleName, nameof(DefaultRoleName));

        return dtoChecker.GetCheckResult();
    }
}
