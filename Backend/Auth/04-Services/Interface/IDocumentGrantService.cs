using Auth.Dto.DocumentGrant;
using Auth.Other;
using Shared.Result;

namespace Auth.Service.Interface;

public interface IDocumentGrantService {
    Task<ApiResult<DocumentGrantDto>> CreateDocumentGrant(CreateDocumentGrantRequest req);
    Task<ApiResult> DeleteDocumentGrant(DeleteDocumentGrantRequest req);

    Task<ApiResult<PaginatedResponse<DocumentGrantDto>>> 
    GetDocumentGrants(DocumentGrantsPagedFilter filter);
    Task<ApiResult> ProvideRoleByGrant(ProvideRoleByGrantRequest req);
    Task<ApiResult> ProvideRoleByRoleName(ProvideRoleByRoleNameRequest req);
}