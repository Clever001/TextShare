using System.Diagnostics.CodeAnalysis;

namespace TextShareApi.Dtos.Greeter;

public class GreetingRequest {
    [NotNull]
    public string Name {get; set;} = "";
}