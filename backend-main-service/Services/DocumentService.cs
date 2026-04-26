using System.Linq.Expressions;
using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Exceptions;
using DocShareApi.Mappers;
using DocShareApi.Models;
using DocShareApi.Repositories;
using Microsoft.IdentityModel.Abstractions;

namespace DocShareApi.Services;

using R = Result;
using RD = Result<Document>;
using RPD = Result<PaginatedResponseDto<Document>>;

public class DocumentService(
    IDocumentRepo docRepo,
    IAccountRepository accRepo,
    ITagRepo tagRepo,
    IUniqueIdService idServ
) : IDocumentService {
    public async Task<RD> CreateDocument(string callerId, CreateUpdateDocDto dto) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return RD.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }
        var newDocId = await idServ.GenerateNewId();

        var newDoc = new Document() {
            Id = newDocId,
            Title = dto.Title,
            Description = dto.Description,
            CreatedOn = DateTime.UtcNow,
            OwnerId = callerId,
            Tags = [],
            UserRoles = [],
            Versions = [],
            PublishedVersion = null,
            Comments = [],
        };

        // Adding Tags
        for (int i = 0; i < dto.Tags.Count; i++) {
            dto.Tags[i] = dto.Tags[i].ToLower();
        }

        if (dto.Tags.Count > 0) {
            var existingTags = new Dictionary<string, Tag>(
                (await tagRepo.GetTags(t => dto.Tags.Contains(t.Name))).Select(t =>
                    new KeyValuePair<string, Tag>(t.Name, t)));

            foreach (var tag in dto.Tags.Distinct().Select(t => t)) {
                newDoc.Tags.Add(existingTags.TryGetValue(tag, out var existingTag)
                    ? existingTag
                    : new Tag { Name = tag });
            }
        }

        // Adding UserRoles
        if (dto.Roles.Count > 0) {
            foreach (var userIdToRole in dto.Roles) {
                newDoc.UserRoles.Add(new UserToDocRole() {
                    UserId = userIdToRole.Key,
                    DocumentId = newDocId,
                    Role = userIdToRole.Value
                });
            }
        }

        await docRepo.Create(newDoc);
        return RD.Success(newDoc);
    }

    public async Task<RD> GetDocumentInfo(string docId) {
        var doc = await docRepo.GetById(docId);
        if (doc == null) {
            return RD.Failure(
                new NotFoundException("Document with such id does not exist")
            );
        }

        doc.UserRoles = [];
        return RD.Success(doc);
    }

    public async Task<RPD> SearchDocuments(
        SortDto sort, PaginationDto pagination, DocumentFilterDto filter
    ) {
        // Pagination
        var skip = (pagination.PageNumber - 1) * pagination.PageSize;
        var take = pagination.PageSize;

        // Filtering
        var predicates = new List<Expression<Func<Document, bool>>> ();
        if (filter.Title != null) {
            predicates.Add(d => d.Title.ToLower().Contains(filter.Title.ToLower()));
        }
        if (filter.Tags is { Count: > 0 }) {
            predicates.Add(d => filter.Tags.All(tag => d.Tags.Any(t => t.Name == tag)));
        }
        if (filter.FromDate != null) {
            predicates.Add(d => d.CreatedOn >= filter.FromDate);
        }
        if (filter.ToDate != null) {
            predicates.Add(d => d.CreatedOn <= filter.ToDate);
        }
        if (filter.OwnerName != null) {
            predicates.Add(d => d.Owner.UserName!.ToLower().Contains(filter.OwnerName.ToLower()));
        }

        // Sorting
        FilterResult<Document> filterResult;
        switch (sort.SortBy) {
            case "title":
                filterResult = await docRepo.GetAllDocuments(new QueryFilter<Document, string>(
                    skip, take, d => d.Title, sort.SortAscending, predicates
                ));
                break;
            case "created_on":
                filterResult = await docRepo.GetAllDocuments(new QueryFilter<Document, DateTime>(
                    skip, take, d => d.CreatedOn, sort.SortAscending, predicates
                ));
                break;
            case "OwnerName":
                filterResult = await docRepo.GetAllDocuments(new QueryFilter<Document, string>(
                    skip, take, d => d.Owner.UserName!, sort.SortAscending, predicates
                ));
                break;
            default:
                return RPD.Failure(new BadRequestException("Invalid SortBy field."));
        }

        return RPD.Success(
            filterResult.ToPaginatedResponse(pagination)
        );
    }

    public Task<RD> UpdateDocumentInfo(CreateUpdateDocDto dto) {
        throw new NotImplementedException();
    }

    public Task<R> DeleteDocument(string callerId, string docId) {
        throw new NotImplementedException();
    }
}