namespace DocShareApi.Dtos.Documents;

public record CreateDocCommand(
    string DocumentId,
    DateTime CreatedOn,
    string OwnerId
);