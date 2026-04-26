using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IDocumentRepo {
    public Task Create(Document doc);
    public Task<Document?> GetById(string docId);
    public Task<FilterResult<Document>> GetAllDocuments<OrderT>(
        QueryFilter<Document, OrderT> filter
    );
    public Task Update(string docId, Document doc);
    public Task Delete(string docId);
}