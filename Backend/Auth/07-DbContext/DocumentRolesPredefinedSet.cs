using System.Collections;
using System.Collections.Immutable;
using Auth.Model;

namespace Auth.DbContext;

public class DocumentRolesPredefinedSet : ICollection<DocumentRole> {
    private readonly static ImmutableArray<DocumentRole> roles = ImmutableArray.Create(
        new DocumentRole() { Name = "Reader",
            CanRead = true, CanComment = false, CanEdit = false,
            CanDelete = false, 
            CanCreateRoleGrants = false, CanManageRoles = false },
        new DocumentRole() { Name = "Commentator",
            CanDelete = false,
            CanRead = true, CanComment = true, CanEdit = false, 
            CanCreateRoleGrants = false, CanManageRoles = false },
        new DocumentRole() { Name = "Editor",
            CanRead = true, CanComment = true, CanEdit = true, 
            CanDelete = false,
            CanCreateRoleGrants = false, CanManageRoles = false },
        new DocumentRole() { Name = "Administrator",
            CanRead = true, CanComment = true, CanEdit = true, 
            CanDelete = true,
            CanCreateRoleGrants = true, CanManageRoles = true }
    );

    public static DocumentRole GetReaderRole() => roles[0];
    public static DocumentRole GetCommentatorRole() => roles[1];
    public static DocumentRole GetEditorRole() => roles[2];
    public static DocumentRole GetAdministratorRole() => roles[3];

    public DocumentRolesPredefinedSet() {}

    public int Count => roles.Length;

    public bool IsReadOnly => true;

    public bool Contains(DocumentRole target) {
        foreach (var role in roles) {
            if (string.Equals(role.Name, target.Name, StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
        }
        return false;
    }

    public IEnumerator<DocumentRole> GetEnumerator() {
        for (int i = 0; i < Count; i++) {
            yield return roles[i];
        }
    }

    public void Add(DocumentRole item) {
        throw new NotSupportedException();
    }

    public void Clear() {
        throw new NotSupportedException();
    }

    public void CopyTo(DocumentRole[] array, int arrayIndex) {
        throw new NotSupportedException();
    }

    public bool Remove(DocumentRole item) {
        throw new NotSupportedException();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}