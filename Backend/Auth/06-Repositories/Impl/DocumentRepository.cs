using System.Net.Sockets;
using Auth.DbContext;
using Auth.Model;
using Auth.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Auth.Repository.Impl;

public class DocumentRepository(
    AppDbContext dbContext
) : IDocumentRepository {
    public async Task<EntityResult> SaveDocumentMetadata(DocumentMetadata document) {
        var errors = new List<string>();

        var containsSameId = await ContainsDocumentMetadataById(document.Id);
        if (containsSameId) {
            errors.Add("DocumentMetadata with the same id already exists.");
        }

        var containsOwner = await ContainsUserById(document.OwnerId);
        if (!containsOwner) {
            errors.Add("User with such id does not exist.");
        }

        var containsDefaultRole = await ContainsDocumentRoleByName(
            document.DefaultRoleId
        );
        if (!containsDefaultRole) {
            errors.Add("DocumentRole with such name does not exist.");
        }

        if (errors.Count == 0) {
            await dbContext.DocumentsMetadata.AddAsync(document);
            return EntityResult.Success();
        } else {
            return EntityResult.Failure(errors);
        }
    }

    public async Task<DocumentMetadata?> FindDocumentMetadata(string documentId) {
        return await dbContext.DocumentsMetadata.FindAsync(documentId);
    }
    
    public async Task<EntityResult> UpdateDefaultRoleForDocument(string documentId, string defaultRoleName) {
        var errors = new List<string>();

        var containsDocumentId = await ContainsDocumentMetadataById(documentId);
        if (!containsDocumentId) {
            errors.Add("DocumentMetadata with such id does not exist.");
        }

        var containsDefaultRole = await ContainsDocumentRoleByName(defaultRoleName);
        if (!containsDefaultRole) {
            errors.Add("DocumentRole with such name does not exist.");
        }

        if (errors.Count == 0) {
            var documentRole = await FindDocumentMetadata(documentId);
            documentRole!.DefaultRoleId = defaultRoleName;
            dbContext.DocumentsMetadata.Update(documentRole);
            await dbContext.SaveChangesAsync();
            return EntityResult.Success();
        } else {
            return EntityResult.Failure(errors);
        }
    }

    public async Task<EntityResult> DeleteMetadataAboutDocument(string documentId) {
        var errors = new List<string>();
        
        var containsDocumentId = await ContainsDocumentMetadataById(documentId);
        if (!containsDocumentId) {
            errors.Add("DocumentMetadata with such id does not exist.");
        }

        if (errors.Count == 0) {
            await DeleteAssignmentsWithSuchDocument(documentId);
            var document = await FindDocumentMetadata(documentId);
            dbContext.DocumentsMetadata.Remove(document!);
            await dbContext.SaveChangesAsync();
            return EntityResult.Success();
        } else {
            return EntityResult.Failure(errors);
        }
    }

    public async Task<DocumentRole?> GetUserRoleForDocument(string documentId, string userId) {
        var errors = new List<string>();

        var containsDocumentId = await ContainsDocumentMetadata
    }

    private async Task<bool> ContainsDocumentMetadataById(string documentId) {
        return await dbContext.DocumentsMetadata.AnyAsync(
            d => d.Id == documentId
        );
    }

    private async Task<bool> ContainsUserById(string userId) {
        // TODO: Replace to other repository.
        return await dbContext.Users.AnyAsync(
            u => u.Id == userId
        );
    }

    private async Task<bool> ContainsDocumentRoleByName(string roleName) {
        // TODO: Replace to other repository.
        return await dbContext.DocumentRoles.AnyAsync(
            r => r.Name == roleName
        );
    }

    private async Task DeleteAssignmentsWithSuchDocument(string documentId) {
        // TODO: Replace to other repository.
        await dbContext.UserDocumentRoleAssignments
            .Where(r => r.TextId == documentId)
            .ExecuteDeleteAsync();
    }
}