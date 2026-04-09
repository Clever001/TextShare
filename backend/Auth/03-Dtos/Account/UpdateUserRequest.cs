using Auth.Other;

namespace Auth.Dto.Account;

public record UpdateUserRequest(
    string InitialUserId,
    string? NewName,
    string? NewEmail
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(InitialUserId, nameof(InitialUserId));
        dtoChecker.AddErrorIfNotNullEmptyString(NewName, nameof(NewName));
        dtoChecker.AddErrorIfNotNullEmptyString(NewEmail, nameof(NewEmail));

        return dtoChecker.GetCheckResult();
    }
}
