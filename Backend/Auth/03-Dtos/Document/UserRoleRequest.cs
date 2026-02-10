using Auth.Other;

namespace Auth.Dto.Document;

public record UserRoleRequest(
    string UserId,
    string DocumentId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.CheckForRequiredString(UserId, nameof(UserId));
        dtoChecker.CheckForRequiredString(DocumentId, nameof(DocumentId));

        return dtoChecker.GetCheckResult();
    }
}
