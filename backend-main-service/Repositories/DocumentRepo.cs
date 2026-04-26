using DocShareApi.Data;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocShareApi.Repositories;

public class DocumentRepo(
    AppDbContext context
) : IDocumentRepo {
    public async Task Create(Document doc) {
        await context.Documents.AddAsync(doc);
        await context.SaveChangesAsync();
    }

    public async Task<Document?> GetById(string docId) {
        var doc = await context.Documents
            .Include(d => d.Tags)
            .Include(d => d.UserRoles)
            .Include(d => d.Comments)
            .Include(d => d.Owner)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == docId);
        return doc;
    }

    public async Task<FilterResult<Document>> GetAllDocuments<OrderT>(QueryFilter<Document, OrderT> filter) {
        IQueryable<Document> docs = context.Documents;

        foreach (var predicate in filter.Predicates ?? []) {
            docs = docs.Where(predicate);
        }
        int count = await docs.CountAsync();
        docs = filter.IsAscending ?
            docs.OrderBy(filter.KeyOrder) :
            docs.OrderByDescending(filter.KeyOrder);
        docs = docs.Skip(filter.Skip).Take(filter.Take);
        var selection = await docs
            .Include(d => d.Tags)
            .Include(d => d.Owner)
            .AsNoTracking()
            .ToListAsync();

        return new FilterResult<Document>(
            count,
            selection
        );
    }

    public async Task Update(string docId, Document doc) {
        Document? foundDoc = await context.Documents.FindAsync(docId);
        if (foundDoc == null) 
            throw new NullReferenceException("Object does not exist");

        foundDoc.Title = doc.Title;
        foundDoc.Description = doc.Description;
        foundDoc.Tags = doc.Tags;
        foundDoc.UserRoles = doc.UserRoles;
        context.Update(foundDoc);
        await context.SaveChangesAsync();
    }

    public async Task Delete(string docId) {
        Document? foundDoc = await context.Documents.FindAsync(docId);
        if (foundDoc == null) 
            throw new NullReferenceException("Object does not exist");

        context.Remove(foundDoc);
        await context.SaveChangesAsync();
    }
}