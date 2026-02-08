using Microsoft.AspNetCore.Identity;
using Shared.Result;

namespace Auth.Other;

public static class IdentityResultExtensions {
    public static EntityResult ToEntityResult(
        this IdentityResult identityResult
    ) {
        if (identityResult.Succeeded) {
            return EntityResult.Success();
        } else {
            return EntityResult.Failure(
                identityResult.Errors.Select(e => e.Description)
            );
        }
    }
}