using Auth.Other;

namespace Auth.Dto.Document;

public record UserRoleRequest(
    string UserId,
    string DocumentId
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(UserId, nameof(UserId));
        dtoChecker.AddErrorIfNullOrEmptyString(DocumentId, nameof(DocumentId));

        return dtoChecker.GetCheckResult();
    }
}
