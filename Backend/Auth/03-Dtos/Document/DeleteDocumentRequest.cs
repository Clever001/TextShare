using Auth.Other;

namespace Auth.Dto.Document;

public record DeleteDocumentRequest(
    string DocumentId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(DocumentId, nameof(DocumentId));

        return dtoChecker.GetCheckResult();
    }
}
