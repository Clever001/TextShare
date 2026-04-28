namespace DocShareApi.Models;

public class DocToTag {
    public string DocumentId { get; set; }
    public Document Document {get; set;}
    public string TagName {get; set;}
    public Tag Tag {get; set;}
}