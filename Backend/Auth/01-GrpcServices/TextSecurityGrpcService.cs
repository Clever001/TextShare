using Auth.Grpc;
using Auth.Mappers;
using Auth.Services.Interfaces;
using Grpc.Core;

namespace Auth.Services;

public class TextSecurityGrpcService(
    ITextSecurityService _service,
    TextSecurityMapper _mapper,
    SharedMapper _sharedMapper
) : TextSecurityGrpc.TextSecurityGrpcBase {
    public override async Task<EmptyResult> PassReadSecurityChecks(PassReadChecksReq request, ServerCallContext context) {
        var textProjection = _mapper.ToTextSecurityProjection(request.Text);
        var result = await _service.PassReadSecurityChecks(
            textProjection, 
            request.HasRequestSenderId ? request.RequestSenderId : null, 
            request.HasPassword ? request.Password : null
        );
        return _sharedMapper.ToEmptyResult(result);
    }

    public override async Task<EmptyResult> PassWriteSecurityChecks(PassWriteChecksReq request, ServerCallContext context) {
        var textProjection = _mapper.ToTextSecurityProjection(request.Text);
        var result = await _service.PassWriteSecurityChecks(
            textProjection, 
            request.RequestSenderId
        );
        return _sharedMapper.ToEmptyResult(result);
    }

    public override async Task<PasswordHash> HashPassword(HashPasswordReq request, ServerCallContext context) {
        var result = await _service.HashPassword(request.UserId, request.Password);
        return _mapper.ToPasswordHash(result);
    }
}