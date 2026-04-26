using System.ComponentModel.DataAnnotations.Schema;

namespace DocShareApi.Models;

[Table("HashSeeds")]
public class HashSeed {
    public int Id { get; set; }
    public ulong NextSeed { get; set; } = 0UL;
}
