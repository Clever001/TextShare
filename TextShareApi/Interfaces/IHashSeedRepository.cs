using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface IHashSeedRepository {
    Task<HashSeed> GetHashSeed();
    Task<HashSeed> GetAndAppendHashSeed();
}