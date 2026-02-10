using Auth.Other;

namespace Auth.Dto.Account;

public record LoginUserRequest(
    string NameOrEmail,
    string Password
) : ICheckable {
    public DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfNullOrEmptyString(NameOrEmail, nameof(NameOrEmail));
        dtoChecker.AddErrorIfNullOrEmptyString(Password, nameof(Password));

        return dtoChecker.GetCheckResult();
    }
}
