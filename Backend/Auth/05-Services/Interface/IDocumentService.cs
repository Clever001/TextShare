using Auth.Dto.Document;
using Shared.Result;

namespace Auth.Service.Interface;

public interface IDocumentService {
    Task<ApiResult<bool>> CanReadDocument(CheckRightRequest req);
    Task<ApiResult<bool>> CanCommentDocument(CheckRightRequest req);
    Task<ApiResult<bool>> CanEditDocument(CheckRightRequest req);
    Task<ApiResult> SaveDocumentMetadata(SaveDocumentRequest documentDto);
    Task<ApiResult> DeleteDocumentMetadata(DeleteDocumentRequest req);
}