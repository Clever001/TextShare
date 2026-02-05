using Auth.Dto.Document;
using Shared;

namespace Auth.Service.Interface;

public interface IDocumentService {
    Task<Result<bool>> CanReadDocument(CheckRightRequest req);
    Task<Result<bool>> CanCommentDocument(CheckRightRequest req);
    Task<Result<bool>> CanEditDocument(CheckRightRequest req);
    Task<Result> SaveDocumentMetadata(SaveDocumentRequest documentDto);
    Task<Result> DeleteDocumentMetadata(DeleteDocumentRequest req);
}