using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Auth.GrpcService.Other;

public class ExceptionInterceptor : Interceptor {
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request, 
        ServerCallContext context, 
        UnaryServerMethod<TRequest, TResponse> continuation
    ) {
        try {
            return await continuation(request, context);
        } catch (Exception ex) {
            throw new RpcException(
                status: new Status(
                    statusCode: StatusCode.Internal, 
                    detail: "Internal server error",
                    debugException: ex
                ), 
                message: ex.Message
            );
        }
    }
}