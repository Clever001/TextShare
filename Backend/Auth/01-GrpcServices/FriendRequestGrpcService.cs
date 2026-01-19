using Auth.Grpc;
using Grpc.Core;

namespace Auth.Services;

public class FriendRequestGrpcService : FriendRequestGrpc.FriendRequestGrpcBase {
    public override Task<FRResult> CreateFR(FRIndex request, ServerCallContext context) {
        return base.CreateFR(request, context);
    }

    public override Task<FRResult> DeleteFR(FRIndex request, ServerCallContext context) {
        return base.DeleteFR(request, context);
    }
    
    public override Task<FRResult> ProcessFR(ProcessFRReq request, ServerCallContext context) {
        return base.ProcessFR(request, context);
    }

    public override Task<FRResult> GetFR(FRIndex request, ServerCallContext context) {
        return base.GetFR(request, context);
    }

    public override Task<PaginatedFRsResult> GetSentFRs(FRFilter request, ServerCallContext context) {
        return base.GetSentFRs(request, context);
    }

    public override Task<PaginatedFRsResult> GetReceivedFRs(FRFilter request, ServerCallContext context) {
        return base.GetReceivedFRs(request, context);
    }
}