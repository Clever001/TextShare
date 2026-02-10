using Auth.Dto;
using Shared.ApiError;

namespace Auth.Service.Impl;

public class ServiceBase {
    protected (bool IsValid, IApiError possibleError) CheckValidity(ICheckable checkable) {
        var validationResult = checkable.CheckValidity();
        var possibleError = new BadRequestApiError(
            "Provided information is invalid.",
            validationResult.Errors
        );
        return (validationResult.IsValid, possibleError);
    }
}