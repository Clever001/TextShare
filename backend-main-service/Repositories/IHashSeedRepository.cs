using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface IHashSeedRepository {
    Task<HashSeed> GetHashSeed();
    Task<HashSeed> GetAndAppendHashSeed();
}
