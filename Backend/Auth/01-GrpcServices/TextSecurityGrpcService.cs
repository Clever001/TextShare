using Auth.Grpc;
using Grpc.Core;

namespace Auth.Services;

public class TextSecurityGrpcService : TextSecurityGrpc.TextSecurityGrpcBase {
    public override Task<EmptyResult> PassReadSecurityChecks(PassReadChecksReq request, ServerCallContext context) {
        return base.PassReadSecurityChecks(request, context);
    }

    public override Task<EmptyResult> PassWriteSecurityChecks(PassWriteChecksReq request, ServerCallContext context) {
        return base.PassWriteSecurityChecks(request, context);
    }

    public override Task<PasswordHash> HashPassword(HashPasswordReq request, ServerCallContext context) {
        return base.HashPassword(request, context);
    }
}