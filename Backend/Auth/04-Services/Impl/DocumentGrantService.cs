using Auth.Dto.DocumentGrant;
using Auth.Other;
using Auth.Repository.Interface;
using Auth.Service.Interface;
using Shared.Result;

namespace Auth.Service.Impl;

public class DocumentGrantService(
    IDocumentGrantRepository documentGrantRepository
) : IDocumentGrantService {
    public Task<ApiResult<DocumentGrantDto>> CreateDocumentGrant(CreateDocumentGrantRequest req) {
        throw new NotImplementedException();
    }

    public Task<ApiResult> DeleteDocumentGrant(DeleteDocumentGrantRequest req) {
        throw new NotImplementedException();
    }

    public Task<ApiResult<PaginatedResponse<DocumentGrantDto>>> GetDocumentGrants(DocumentGrantsPagedFilter filter) {
        throw new NotImplementedException();
    }

    public Task<ApiResult> ProvideRoleByGrant(ProvideRoleByGrantRequest req) {
        throw new NotImplementedException();
    }

    public Task<ApiResult> ProvideRoleByRoleName(ProvideRoleByRoleNameRequest req) {
        throw new NotImplementedException();
    }
}