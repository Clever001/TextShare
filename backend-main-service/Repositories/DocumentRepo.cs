using DocShareApi.Data;
using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.Enums;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DocShareApi.Repositories;

public class DocumentRepo(
    AppDbContext context
) : IDocumentRepo {
    public async Task Create(CreateDocCommand command, CreateUpdateDocDto dto) {
        using var transaction = await context.Database.BeginTransactionAsync();
        try {
            var newDoc = new Document() {
                Id = command.DocumentId,
                Title = dto.Title,
                Description = dto.Description,
                CreatedOn = command.CreatedOn,
                OwnerId = command.OwnerId,
            };
            await context.Documents.AddAsync(newDoc);

            await AddDocToTags(command.DocumentId, dto.Tags);
            await AddUserRoles(command.DocumentId, dto.Roles);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        } catch {
            await transaction.RollbackAsync();
            throw;
        }
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

    public async Task<FilterResult<Document>> GetAllDocuments<OrderT>(
        QueryFilter<Document, OrderT> filter
    ) {
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

    public async Task Update(string docId, CreateUpdateDocDto dto) {
        Document? foundDoc = await context.Documents.FindAsync(docId);
        if (foundDoc == null)
            throw new NullReferenceException("Object does not exist");

        using var transaction = await context.Database.BeginTransactionAsync();
        try {
            // Updating Tags
            {
                await context.DocToTags.Where(dt => dt.DocumentId == docId)
                    .ExecuteDeleteAsync();
                await AddDocToTags(docId, dto.Tags);
            }

            // Updating UserRoles
            {
                await context.UserToDocRoles.Where(r => r.DocumentId == docId)
                    .ExecuteDeleteAsync();
                await AddUserRoles(docId, dto.Roles);
            }

            // Versions, PublishedVersions, Comments tables does not
            // update here

            foundDoc.Title = dto.Title;
            foundDoc.Description = dto.Description;
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        } catch {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task Delete(string docId) {
        Document? foundDoc = await context.Documents.FindAsync(docId);
        if (foundDoc == null)
            throw new NullReferenceException("Object does not exist");

        using var transaction = await context.Database.BeginTransactionAsync();
        try {
            await context.DocToTags.Where(dt => dt.DocumentId == docId)
                .ExecuteDeleteAsync();
            await context.UserToDocRoles.Where(r => r.DocumentId == docId)
                .ExecuteDeleteAsync();
            await context.DocVerions.Where(v => v.DocumentId == docId)
                .ExecuteDeleteAsync();
            await context.PublishedVersion.Where(v => v.DocumentId == docId)
                .ExecuteDeleteAsync();
            await context.Comments.Where(c => c.DocumentId == docId)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        } catch {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task AddDocToTags(string docId, List<string> TagNames) {
        var existingTagNames = await context.Tags
            .Where(t => TagNames.Contains(t.Name))
            .Select(t => t.Name)
            .ToHashSetAsync();
        var nonExistingTags = TagNames
            .Where(t => !existingTagNames.Contains(t))
            .Select(tn => new Tag() { Name = tn });
        await context.Tags.AddRangeAsync(nonExistingTags);
        var newDocToTags = TagNames
            .Select(tn => new DocToTag() {
                DocumentId = docId,
                TagName = tn
            });
        await context.DocToTags.AddRangeAsync(newDocToTags);
    }

    private async Task AddUserRoles(string docId, Dictionary<string, UserDevRole> roles) {
        var newUserToDocRoles = roles
            .Select(ur => new UserToDocRole() {
                UserId = ur.Key,
                DocumentId = docId,
                Role = ur.Value
            });
        await context.UserToDocRoles.AddRangeAsync(newUserToDocRoles);
    }
}