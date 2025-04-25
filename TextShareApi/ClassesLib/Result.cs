namespace TextShareApi.ClassesLib;

public class Result<T> {
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }
    public bool IsClientError { get; }

    private Result(bool isSuccess, T value, string error, bool isClientError) {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        IsClientError = isClientError;
    }

    public Result ToSimpleResult() {
        if (IsSuccess) {
            return Result.Success();
        }
        return Result.Failure(Error, IsClientError);
    }

    public static Result<T> Success(T value) {
        return new Result<T>(true, value, null!, default);
    }

    public static Result<T> Failure(string message, bool isClientError) {
        if (string.IsNullOrWhiteSpace(message)) {
            throw new ArgumentException("Error message cannot be null or empty.", nameof(message));
        }
        return new Result<T>(false, default!, message, isClientError);
    }
}

public class Result {
    public bool IsSuccess { get; }
    public string Error { get; }
    public bool IsClientError { get; }

    private Result(bool isSuccess, string error, bool isClientError) {
        IsSuccess = isSuccess;
        Error = error;
        IsClientError = isClientError;
    }

    public Result<T> ToGenericResult<T>(T value) {
        if (IsSuccess) {
            return Result<T>.Success(value);
        }
        return Result<T>.Failure(Error, IsClientError);
    }

    public static Result Success() {
        return new Result(true, null!, default);
    }

    public static Result Failure(string message, bool isClientError) {
        if (string.IsNullOrWhiteSpace(message)) {
            throw new ArgumentException("Error message cannot be null or empty.", nameof(message));
        }
        return new Result(false, message, isClientError);
    }
}