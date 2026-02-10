using Auth.Grpc;
using Grpc.Core;

namespace Auth.GrpcService;

public class DocumentGrpcService : DocumentGrpc.DocumentGrpcBase {
    public override Task<EmptyGrpcResult> SaveDocumentMetadata(SaveDocumentGrpcRequest request, ServerCallContext context) {
        return base.SaveDocumentMetadata(request, context);
    }

    public override Task<EmptyGrpcResult> UpdateDefaultRoleForDocument(UpdateDefaultRoleGrpcRequest request, ServerCallContext context) {
        return base.UpdateDefaultRoleForDocument(request, context);
    }

    public override Task<EmptyGrpcResult> DeleteDocumentMetadata(DeleteDocumentGrpcRequest request, ServerCallContext context) {
        return base.DeleteDocumentMetadata(request, context);
    }

    public override Task<UserRoleGrpcResult> GetUserRoleForDocument(UserRoleGrpcRequest request, ServerCallContext context) {
        return base.GetUserRoleForDocument(request, context);
    }
}