using Auth.Other;

namespace Auth.Dto.Document;

public record SaveDocumentRequest(
    string DocumentId,
    string CreatorId,
    string DefaultRoleName
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(DocumentId, nameof(DocumentId));
        dtoChecker.CheckForRequiredString(CreatorId, nameof(CreatorId));
        dtoChecker.CheckForRequiredString(DefaultRoleName, nameof(DefaultRoleName));

        return dtoChecker.GetCheckResult();
    }
}
