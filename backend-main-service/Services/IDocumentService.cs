using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Services;

public interface IDocumentService {
    Task<Result<Document>> CreateDocument(string callerId, CreateUpdateDocDto dto);
    Task<Result<Document>> GetDocumentInfo(string docId);
    Task<Result<PaginatedResponseDto<Document>>> SearchDocuments(
        SortDto sort, PaginationDto pagination, DocumentFilterDto filter
    );
    Task<Result<Document>> UpdateDocumentInfo(string callerId, string documentId, CreateUpdateDocDto dto);
    Task<Result> DeleteDocument(string callerId, string docId);
}