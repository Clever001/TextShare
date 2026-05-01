using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IDocumentRepo {
    Task Create(CreateDocCommand command, CreateUpdateDocDto dto);
    Task<Document?> GetById(string docId);
    Task<bool> ContainsById(string docId);
    Task<bool> ContainsByTitleAndOwner(string title, string ownerId);
    Task<FilterResult<Document>> GetAll<OrderT>(
        QueryFilter<Document, OrderT> filter
    );
    Task Update(string docId, CreateUpdateDocDto dto);
    Task Delete(string docId);
}
