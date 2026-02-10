using Auth.Other;

namespace Auth.Dto.Account;

public record RegisterUserRequest(
    string Name,
    string Email,
    string Password
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(Name, nameof(Name));
        dtoChecker.AddErrorIfNullOrEmptyString(Email, nameof(Email));
        dtoChecker.AddErrorIfNotEmail(Email, nameof(Email));
        dtoChecker.AddErrorIfNullOrEmptyString(Password, nameof(Password));

        return dtoChecker.GetCheckResult();
    }
}
