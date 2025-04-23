using TextShareApi.Models;
using TextShareApi.Models.Enums;

namespace TextShareApi.ClassesLib;

public class SecurityResponse : ResponseBase {
    public SecurityCheckResult Details { get; }
    public Text? Text { get; }

    public SecurityResponse(SecurityCheckResult checkResult, Text? text = null) : base(text is not null) {
        if (checkResult == SecurityCheckResult.Allowed && text is null) {
            throw new InvalidDataException("Security check result is invalid");
        }
        Details = checkResult;
        Text = text;
    }
}