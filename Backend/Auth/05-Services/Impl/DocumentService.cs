using Auth.Dto.Document;
using Auth.Repository.Interface;
using Auth.Service.Interface;
using Shared.Result;

namespace Auth.Service.Impl;

public class DocumentService(
    IDocumentRepository documentRepository
) : IDocumentService {
    public Task<ApiResult> SaveDocumentMetadata(SaveDocumentRequest req) {
        throw new NotImplementedException();
    }

    public Task<ApiResult> UpdateDefaultRoleForDocument(UpdateDefaultRoleRequest req) {
        throw new NotImplementedException();
    }

    public Task<ApiResult> DeleteDocumentMetadata(DeleteDocumentRequest req) {
        throw new NotImplementedException();
    }

    public Task<ApiResult<UserRoleDto>> GetUserRoleForDocument(UserRoleRequest req) {
        throw new NotImplementedException();
    }
}