using Shared.ApiError;

namespace Shared;

public class Result<T> {
    private Result(bool isSuccess, T value, ApiError.IApiError error) {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public IApiError Error { get; }

    public Result ToSimpleResult() {
        if (IsSuccess) return Result.Success();
        return Result.Failure(Error);
    }

    public static Result<T> Success(T value) {
        return new Result<T>(true, value, default!);
    }

    public static Result<T> Failure(ApiError.IApiError error) {
        return new Result<T>(false, default!, error);
    }
}

public class Result {
    private Result(bool isSuccess, ApiError.IApiError error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public IApiError Error { get; }

    public Result<T> ToGenericResult<T>(T value) {
        if (IsSuccess) return Result<T>.Success(value);
        return Result<T>.Failure(Error);
    }

    public static Result Success() {
        return new Result(true, default!);
    }

    public static Result Failure(ApiError.IApiError error) {
        return new Result(false, error);
    }
}