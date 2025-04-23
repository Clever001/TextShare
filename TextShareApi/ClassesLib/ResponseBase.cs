namespace TextShareApi.ClassesLib;

public abstract class ResponseBase {
    public bool IsSuccess { get; }

    public ResponseBase(bool isSuccess) {
        IsSuccess = isSuccess;
    }
}