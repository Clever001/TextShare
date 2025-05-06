using TextShareApi.Exceptions;

namespace TextShareApi.ClassesLib;

public class Result<T> {
    private Result(bool isSuccess, T value, ApiException exception) {
        IsSuccess = isSuccess;
        Value = value;
        Exception = exception;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public ApiException Exception { get; }

    public Result ToSimpleResult() {
        if (IsSuccess) return Result.Success();
        return Result.Failure(Exception);
    }

    public static Result<T> Success(T value) {
        return new Result<T>(true, value,  default!);
    }

    public static Result<T> Failure(ApiException exception) {
        return new Result<T>(false, default!, exception);
    }
}

public class Result {
    private Result(bool isSuccess, ApiException exception) {
        IsSuccess = isSuccess;
        Exception = exception;
    }

    public bool IsSuccess { get; }
    public ApiException Exception { get; }

    public Result<T> ToGenericResult<T>(T value) {
        if (IsSuccess) return Result<T>.Success(value);
        return Result<T>.Failure(Exception);
    }

    public static Result Success() {
        return new Result(true, default!);
    }

    public static Result Failure(ApiException exception) {
        return new Result(false, exception);
    }
}