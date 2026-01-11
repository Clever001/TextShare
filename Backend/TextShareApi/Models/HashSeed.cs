namespace TextShareApi.Models;

public class HashSeed {
    public int Id { get; set; }
    public ulong NextSeed { get; set; } = 0UL;
}