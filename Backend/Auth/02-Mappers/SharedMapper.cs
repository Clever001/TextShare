using Auth.Grpc;
using Auth.Other;
using Shared;
using Shared.ApiError;

namespace Auth.Mapper;

public static class SharedMapper {
    public static TResult ConvertToGrpcResult<TConvertable, TResult>(
        Result<TConvertable> serviceOpResult,
        Func<TConvertable, TResult> onSuccess,
        Func<IApiError, TResult> onFailure
    ) {
        if (serviceOpResult.IsSuccess) {
            var convertable = serviceOpResult.Value;
            return onSuccess(convertable);
        } else {
            var apiError = serviceOpResult.Error;
            return onFailure(apiError);
        }
    }

    public static GrpcException ConvertToGrpcDto(IApiError apiError) {
        var grpcException = new GrpcException() {
            Code = apiError.Code,
            CodeNumber = apiError.CodeNumber,
            Description = apiError.Description
        };
        grpcException.Details.AddRange(apiError.Details);
        return grpcException;
    }

    public static PaginationGrpcResponse ConvertToGrpcDto<T>(PaginatedResponse<T> page) {
        return new PaginationGrpcResponse() {
            TotalItems = page.TotalItems,
            TotalPages = page.TotalPages,
            CurrentPage = page.CurrentPage,
            PageSize = page.PageSize
        };
    }
}