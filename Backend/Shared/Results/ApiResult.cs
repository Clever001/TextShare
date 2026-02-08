using Shared.ApiError;

namespace Shared.Result;

public class ApiResult<T> {
    private ApiResult(bool isSuccess, T value, IApiError error) {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public IApiError Error { get; }

    public ApiResult ToSimpleResult() {
        if (IsSuccess) return ApiResult.Success();
        return ApiResult.Failure(Error);
    }

    public static ApiResult<T> Success(T value) {
        return new ApiResult<T>(true, value, default!);
    }

    public static ApiResult<T> Failure(IApiError error) {
        return new ApiResult<T>(false, default!, error);
    }
}

public class ApiResult {
    private ApiResult(bool isSuccess, IApiError error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public IApiError Error { get; }

    public ApiResult<T> ToGenericResult<T>(T value) {
        if (IsSuccess) return ApiResult<T>.Success(value);
        return ApiResult<T>.Failure(Error);
    }

    public static ApiResult Success() {
        return new ApiResult(true, default!);
    }

    public static ApiResult Failure(IApiError error) {
        return new ApiResult(false, error);
    }
}