using Auth.Grpc;
using Auth.Mappers;
using Auth.Services.Interfaces;
using Grpc.Core;

namespace Auth.Services;

public class FriendRequestGrpcService(
    IFriendRequestService _service,
    FriendRequestGrpcMapper _mapper,
    SharedMapper _sharedMapper
) : FriendRequestGrpc.FriendRequestGrpcBase {
    public override async Task<FRResult> CreateFR(FRIndex request, ServerCallContext context) {
        var result = await _service.Create(request.SenderName, request.RecipientName);
        return _mapper.ToFRResult(result);
    }

    public override async Task<EmptyResult> DeleteFR(FRIndex request, ServerCallContext context) {
        var result = await _service.Delete(request.SenderName, request.RecipientName);
        return _sharedMapper.ToEmptyResult(result);
    }
    
    public override async Task<FRResult> ProcessFR(ProcessFRReq request, ServerCallContext context) {
        var result = await _service.Process(
            request.Index.SenderName, 
            request.Index.RecipientName, 
            request.AcceptRequest
        );
        return _mapper.ToFRResult(result);
    }

    public override async Task<FRResult> GetFR(FRIndex request, ServerCallContext context) {
        var result = await _service.GetFriendRequest(request.SenderName, request.RecipientName);
        return _mapper.ToFRResult(result);
    }

    public override async Task<PaginatedFRsResult> GetSentFRs(SentFRFilter request, ServerCallContext context) {
        var pagination = _sharedMapper.ToPaginationCommand(request.PaginationPage);
        var result = await _service.GetSentFriendRequests(
            pagination, 
            request.IsAscending, 
            request.SenderName, 
            request.HasRecipientName ? request.RecipientName : null
        );
        return _mapper.ToPaginatedFRsResult(result);
    }

    public override async Task<PaginatedFRsResult> GetReceivedFRs(RecievedRFilter request, ServerCallContext context) {
        var pagination = _sharedMapper.ToPaginationCommand(request.PaginationPage);
        var result = await _service.GetReceivedFriendRequests(
            pagination,
            request.IsAscending,
            request.HasSenderName ? request.SenderName : null,
            request.RecipientName
        );
        return _mapper.ToPaginatedFRsResult(result);
    }
}