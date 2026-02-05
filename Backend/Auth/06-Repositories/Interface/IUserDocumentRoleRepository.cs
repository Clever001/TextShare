using Auth.Model;

namespace Auth.Repository.Interface;

public interface IUserDocumentRoleRepository {
    Task<UserDocumentRoleAssignment> CreateRelation(UserDocumentRoleAssignment relation);
    Task<UserDocumentRoleAssignment> GetRelation(string userId, string DocumentId);
    Task<UserDocumentRoleAssignment> UpdateRelation(UserDocumentRoleAssignment relation);
    Task DeleteRelation(UserDocumentRoleAssignment relation);
}