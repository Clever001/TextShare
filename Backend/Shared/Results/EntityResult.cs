using System.Collections.Specialized;

namespace Shared.Result;

// TODO: Mayby I template class is not needed?
public class EntityResult<T> {
    private EntityResult(
        bool isSuccess, 
        T value, 
        IEnumerable<string> details
    ) {
        IsSuccess = isSuccess;
        Value = value;
        Details = details;
    }

    public bool IsSuccess { get; }
    public T Value { get; }
    public IEnumerable<string> Details { get; }

    public static EntityResult<T> Success(T value) {
        return new EntityResult<T>(
            isSuccess: true,
            value: value,
            details: default!
        );
    }

    public static EntityResult<T> Failure(IEnumerable<string> details) {
        return new EntityResult<T> (
            isSuccess: false,
            value: default!,
            details: details.ToArray()
        );
    }
}

public class EntityResult {
    private EntityResult(
        bool isSuccess, 
        IEnumerable<string> errorDetails
    ) {
        IsSuccess = isSuccess;
        ErrorDetails = errorDetails;
    }

    public bool IsSuccess { get; }
    public IEnumerable<string> ErrorDetails { get; }

    public static EntityResult Success() {
        return new EntityResult(
            isSuccess: true,
            errorDetails: default!
        );
    }

    public static EntityResult Failure(IEnumerable<string> details) {
        return new EntityResult (
            isSuccess: false,
            errorDetails: details.ToArray()
        );
    }
}