using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface IHashSeedRepository {
    Task<HashSeed> GetHashSeed();
    Task<HashSeed> GetAndAppendHashSeed();
}