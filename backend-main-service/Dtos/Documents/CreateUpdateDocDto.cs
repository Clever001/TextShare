using System.ComponentModel.DataAnnotations;
using DocShareApi.Dtos.Enums;
using DocShareApi.Models;

namespace DocShareApi.Dtos.Documents;

public class CreateUpdateDocDto {
    [Required]
    [Length(1, 70)]
    public string Title {get; init;} = "";
    [Required]
    [MaxLength(250)]
    public string Description {get; init;} = "";
    [Required]
    public List<string> Tags {get; init;} = [];
    [Required]
    public Dictionary<string, UserDevRole> Roles {get; init;} = [];
}
