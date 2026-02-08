using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Auth.CustomException;

public class DbCallException : Exception {
    private static readonly string defaultMessage
        = "No message provided.";

    public DbCallException(
        string? message
    ) : base(message ?? defaultMessage) {

    }

    public DbCallException(
        string? message,
        Exception? innerException
    ) : base(message ?? defaultMessage, innerException) {
        
    }

    public static DbCallException 
    CreateFromIdentityErrors(
        string? message,
        IEnumerable<IdentityError>? errors
    ) {
        var fullMessageBuilder = new StringBuilder();
        fullMessageBuilder.AppendLine(message ?? defaultMessage);
        fullMessageBuilder.AppendLine();

        if (errors != null) {
            fullMessageBuilder.AppendLine("Identity errors information:");
            foreach (var error in errors) {
                fullMessageBuilder.AppendLine(
                    ParseIdentityErrorToString(error)
                );
            }
        }

        return new DbCallException(fullMessageBuilder.ToString());
    }

    private static string ParseIdentityErrorToString(IdentityError error) {
        return $$"""
                {
                    "{{nameof(error.Code)}}": "{{error.Code}}";
                    "{{nameof(error.Description)}}": ""{{error.Description}};
                }
                """;
    }
}