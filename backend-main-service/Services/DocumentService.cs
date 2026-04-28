using System.Linq.Expressions;
using DocShareApi.ClassesLib;
using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.Enums;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Exceptions;
using DocShareApi.Mappers;
using DocShareApi.Models;
using DocShareApi.Repositories;

namespace DocShareApi.Services;

using R = Result;
using RD = Result<Document>;
using RPD = Result<PaginatedResponseDto<Document>>;

public class DocumentService(
    IDocumentRepo docRepo,
    IAccountRepository accRepo,
    IDevRolesRepo rolesRepo,
    IUniqueIdService idServ,
    ILogger<DocumentRepo> logger
) : IDocumentService {
    public async Task<RD> CreateDocument(string callerId, CreateUpdateDocDto dto) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return RD.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }
        
        var createCommand = new CreateDocCommand(
            DocumentId: await idServ.GenerateNewId(),
            CreatedOn: DateTime.UtcNow,
            OwnerId: callerId
        );

        try {
            await docRepo.Create(createCommand, dto);
            var newDoc = await docRepo.GetById(createCommand.DocumentId);
            if (newDoc == null)
                throw new NullReferenceException("Created document not found");
            return RD.Success(newDoc);
        } catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            return RD.Failure(new ServerException());
        }
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

    public async Task<RD> UpdateDocumentInfo(
        string callerId, string documentId, CreateUpdateDocDto dto
    ) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return RD.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }
        var doc = await docRepo.GetById(documentId);
        if (doc == null) {
            return RD.Failure(new NotFoundException());
        }
        var callerRole = await rolesRepo.GetUserToDocRole(callerId, documentId);
        if (callerRole != UserDevRole.Administrator) {
            return RD.Failure(new ForbiddenException());
        }

        try {
            await docRepo.Update(documentId, dto);
            return RD.Success(doc);
        } catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            return RD.Failure(new ServerException());
        }
    }

    public async Task<R> DeleteDocument(string callerId, string documentId) {
        if (!await accRepo.ContainsAccountById(callerId)) {
            return R.Failure(
                new BadRequestException("User with such id does not exist")
            );
        }
        var doc = await docRepo.GetById(documentId);
        if (doc == null) {
            return R.Failure(new NotFoundException());
        }
        if (doc.OwnerId != callerId) {
            return R.Failure(
                new ForbiddenException()
            );
        }

        try {
            await docRepo.Delete(documentId);
            return R.Success();
        } catch (Exception ex) {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            return R.Failure(
                new ServerException()
            );
        }
    }
}