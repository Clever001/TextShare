using DocShareApi.Models;

namespace DocShareApi.Interfaces.Repositories;

public interface IHashSeedRepository {
    Task<HashSeed> GetHashSeed();
    Task<HashSeed> GetAndAppendHashSeed();
}