using TextShareApi.Dtos.Text;
using TextShareApi.Models;

namespace TextShareApi.Mappers;

public static class TextMapper {
    public static TextDto ToTextDto(this Text text) {
        return new TextDto {
            Id = text.Id,
            Title = text.Title,
            Description = text.Description,
            Syntax = text.Syntax,
            Tags = text.Tags.Select(t => t.Name).ToList(),
            Content = text.Content,
            CreatedOn = text.CreatedOn,
            UpdatedOn = text.UpdatedOn,
            OwnerName = text.Owner.UserName!,
            AccessType = text.TextSecuritySettings.AccessType.ToString(),
            HasPassword = text.TextSecuritySettings.Password != null
        };
    }

    public static TextWithoutContentDto ToTextWithoutContentDto(this Text text) {
        return new TextWithoutContentDto {
            Id = text.Id,
            Title = text.Title,
            Description = text.Description,
            Syntax = text.Syntax,
            Tags = text.Tags.Select(t => t.Name).ToList(),
            CreatedOn = text.CreatedOn,
            UpdatedOn = text.UpdatedOn,
            OwnerName = text.Owner.UserName!,
            AccessType = text.TextSecuritySettings.AccessType.ToString(),
            HasPassword = text.TextSecuritySettings.Password != null
        };
    }
}