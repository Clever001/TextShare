using DocShareApi.Dtos.Enums;

namespace DocShareApi.Dtos.Documents;

public record DocumentDto(
    string Id,
    string Title,
    string Description,
    DateTime CreatedOn,
    string OwnerName,
    List<string> Tags,
    Dictionary<string, UserDevRole> UserNamesToRoles
);