using Auth.Grpc;
using Grpc.Core;

namespace Auth.GrpcService;

public class DocumentGrpcService : DocumentGrpc.DocumentGrpcBase {
    public override Task<PermissionGrpcResult> CanReadDocument(CheckRightGrpcReq request, ServerCallContext context) {
        return base.CanReadDocument(request, context);
    }

    public override Task<PermissionGrpcResult> CanCommentDocument(CheckRightGrpcReq request, ServerCallContext context) {
        return base.CanCommentDocument(request, context);
    }

    public override Task<PermissionGrpcResult> CanEditDocument(CheckRightGrpcReq request, ServerCallContext context) {
        return base.CanEditDocument(request, context);
    }

    public override Task<EmptyGrpcResult> SaveDocumentMetadata(SaveDocumentGrpcReq request, ServerCallContext context) {
        return base.SaveDocumentMetadata(request, context);
    }

    public override Task<EmptyGrpcResult> DeleteDocumentMetadata(DeleteDocumentGrpcReq request, ServerCallContext context) {
        return base.DeleteDocumentMetadata(request, context);
    }
}