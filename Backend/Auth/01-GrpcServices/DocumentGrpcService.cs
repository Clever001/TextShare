using Auth.Grpc;
using Grpc.Core;

namespace Auth.GrpcService;

public class DocumentGrpcService : DocumentGrpc.DocumentGrpcBase {
    public override Task<EmptyGrpcResult> SaveDocumentMetadata(SaveDocumentGrpcReq request, ServerCallContext context) {
        return base.SaveDocumentMetadata(request, context);
    }

    public override Task<EmptyGrpcResult> UpdateDefaultRoleForDocument(UpdateDefaultRoleGrpcReq request, ServerCallContext context) {
        return base.UpdateDefaultRoleForDocument(request, context);
    }

    public override Task<EmptyGrpcResult> DeleteDocumentMetadata(DeleteDocumentGrpcReq request, ServerCallContext context) {
        return base.DeleteDocumentMetadata(request, context);
    }

    public override Task<UserRoleGrpcResult> GetUserRoleForDocument(UserRoleGrpcRequest request, ServerCallContext context) {
        return base.GetUserRoleForDocument(request, context);
    }
}