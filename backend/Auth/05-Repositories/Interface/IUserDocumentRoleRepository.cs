using Auth.Model;
using Auth.Other;

namespace Auth.Repository.Interface;

public interface IUserDocumentRoleRepository {
    Task CreateRelation(UserDocumentRoleAssignment relation);
    Task<UserDocumentRoleAssignment?> GetRelation(string userId, string documentId);
    Task<bool> ContainsRelation(string userId, string documentId);
    Task<SelectionOfItems<UserDocumentRoleAssignment>> GetRelations<KeyOrderT>(
        QueryFilter<UserDocumentRoleAssignment, KeyOrderT> queryFilter
    );
    Task UpdateRelation(UserDocumentRoleAssignment relation);
    Task DeleteRelation(UserDocumentRoleAssignment relation);
}