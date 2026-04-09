using Microsoft.AspNetCore.Identity;

namespace Auth.Model;

public class User : IdentityUser {
    public List<DocumentMetadata> CreatedTexts {get; set;} = default!;
}