using Auth.Dto.DocumentGrant;
using Auth.Model;
using Auth.Other;
using Shared;

namespace Auth.Service.Interface;

public interface IDocumentGrantService {
    Task<Result<DocumentRoleGrant>> CreateDocumentGrant(CreateDocumentGrantRequest req);
    Task<Result> DeleteDocumentGrant(DeleteDocumentGrantRequest req);
    Task<Result<PaginatedResponse<DocumentRoleGrant>>> GetAllDocumentGrants
        (DocumentGrantsFilter filter);
    Task<Result> ProvideRoleByGrant(ProvideRoleByGrantRequest req);
    Task<Result> ProvideRoleByRoleName(ProvideRoleByRoleNameRequest req);
}