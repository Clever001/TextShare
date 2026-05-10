namespace DocShareApi.Dtos.QueryOptions.Filters;

public class DocumentFilterDto {
    public string? Title {get; init;}
    public List<string>? Tags {get; init;}
    public DateTime? FromDate {get; set; }
    public DateTime? ToDate {get; set;}
    public string? OwnerName {get;init;}
    public string? OwnerId {get; init;}
}
