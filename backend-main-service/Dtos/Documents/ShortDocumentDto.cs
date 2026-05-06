namespace DocShareApi.Dtos.Documents;

public record ShortDocumentDto(
    string Id,
    string Title,
    string Description,
    DateTime CreatedOn,
    string OwnerName,
    List<string> Tags
);