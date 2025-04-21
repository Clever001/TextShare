namespace TextShareApi.Models;

public class Result<T> {
    public bool IsSuccess { get; }
    public T Value { get; }
    public string Error { get; }

    private Result(bool isSuccess, T value, string error) {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) {
        return new Result<T>(true, value, null!);
    }

    public static Result<T> Failure(string message) {
        if (string.IsNullOrWhiteSpace(message)) {
            throw new ArgumentException("Error message cannot be null or empty.", nameof(message));
        }
        return new Result<T>(false, default!, message);
    }
}

public class Result {
    public bool IsSuccess { get; }
    public string Error { get; }

    private Result(bool isSuccess, string error) {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() {
        return new Result(true, null!);
    }

    public static Result Failure(string error) {
        if (string.IsNullOrWhiteSpace(error)) {
            throw new ArgumentException("Error message cannot be null or empty.", nameof(error));
        }
        return new Result(false, error);
    }
}