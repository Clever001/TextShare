using Auth.Dto.Document;
using Shared.Result;

namespace Auth.Service.Interface;

public interface IDocumentService {
    Task<ApiResult> SaveDocumentMetadata(SaveDocumentRequest req);
    Task<ApiResult> UpdateDefaultRoleForDocument(UpdateDefaultRoleRequest req);
    Task<ApiResult> DeleteDocumentMetadata(DeleteDocumentRequest req);
    Task<ApiResult<UserRoleDto>> GetUserRoleForDocument(UserRoleRequest req);
}