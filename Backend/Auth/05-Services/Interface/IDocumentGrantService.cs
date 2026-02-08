using Auth.Dto.DocumentGrant;
using Auth.Model;
using Auth.Other;
using Shared.Result;

namespace Auth.Service.Interface;

public interface IDocumentGrantService {
    Task<ApiResult<DocumentRoleGrant>> CreateDocumentGrant(CreateDocumentGrantRequest req);
    Task<ApiResult> DeleteDocumentGrant(DeleteDocumentGrantRequest req);
    Task<ApiResult<PaginatedResponse<DocumentRoleGrant>>> GetAllDocumentGrants
        (DocumentGrantsFilter filter);
    Task<ApiResult> ProvideRoleByGrant(ProvideRoleByGrantRequest req);
    Task<ApiResult> ProvideRoleByRoleName(ProvideRoleByRoleNameRequest req);
}