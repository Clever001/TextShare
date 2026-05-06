using DocShareApi.Dtos.Documents;
using DocShareApi.Dtos.Enums;
using DocShareApi.Models;

namespace DocShareApi.Mappers;

public static class DocumentMapper {
    public static FullDocumentDto ToFullDto(this Document doc) {
        return new FullDocumentDto(
            Id: doc.Id,
            Title: doc.Title,
            Description: doc.Description,
            CreatedOn: doc.CreatedOn,
            OwnerName: doc.Owner.UserName!,
            Tags: doc.Tags.Select(t => t.Name).ToList(),
            UserNamesToRoles: doc.UserRoles
                .Select(r => new KeyValuePair<string, UserDevRole>(r.UserId, r.Role))
                .ToDictionary()
        );
    }

    public static ShortDocumentDto ToShortDto(this Document doc) {
        return new ShortDocumentDto(
            Id: doc.Id,
            Title: doc.Title,
            Description: doc.Description,
            CreatedOn: doc.CreatedOn,
            OwnerName: doc.Owner.UserName!,
            Tags: doc.Tags.Select(t => t.Name).ToList()
        );
    }
}
