using Auth.Grpc;
using Grpc.Core;

namespace Auth.GrpcService;

public class DocumentGrantGGrpcService : DocumentGrantGrpc.DocumentGrantGrpcBase {
    public override Task<DocumentGrantGrpcResult> CreateDocumentGrant(CreateDocumentGrantGrpcRequest request, ServerCallContext context) {
        return base.CreateDocumentGrant(request, context);
    }

    public override Task<EmptyGrpcResult> DeleteDocumentGrant(DeleteDocumentGrantGrpcRequest request, ServerCallContext context) {
        return base.DeleteDocumentGrant(request, context);
    }

    public override Task<PagedDocumentGrantsGrpcResult> GetDocumentGrants(DocumentGrantsGrpcPagedFilter request, ServerCallContext context) {
        return base.GetDocumentGrants(request, context);
    }

    public override Task<EmptyGrpcResult> ProvideRoleByGrant(ProvideRoleByGrantGrpcRequest request, ServerCallContext context) {
        return base.ProvideRoleByGrant(request, context);
    }

    public override Task<EmptyGrpcResult> ProvideRoleByRoleName(ProvideRoleByRoleNameGrpcRequest request, ServerCallContext context) {
        return base.ProvideRoleByRoleName(request, context);
    }
}