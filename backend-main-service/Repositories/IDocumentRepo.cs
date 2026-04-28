using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IDocumentRepo {
    public Task Create(CreateDocCommand command, CreateUpdateDocDto dto);
    public Task<Document?> GetById(string docId);
    public Task<FilterResult<Document>> GetAllDocuments<OrderT>(
        QueryFilter<Document, OrderT> filter
    );
    public Task Update(string docId, CreateUpdateDocDto dto);
    public Task Delete(string docId);
}