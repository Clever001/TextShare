using Microsoft.EntityFrameworkCore;
using TextShareApi.Data;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Models;

namespace TextShareApi.Repositories;

public class HashSeedRepository : IHashSeedRepository {
    private readonly AppDbContext _context;

    public HashSeedRepository(AppDbContext context) {
        _context = context;
    }
    
    public async Task<HashSeed> GetHashSeed() {
        var seed = _context.HashSeeds.SingleOrDefault();
        if (seed is null) {
            await _context.HashSeeds.ExecuteDeleteAsync();
            seed = new();
            await _context.HashSeeds.AddAsync(seed);
            await _context.SaveChangesAsync();
        }

        return seed;
    }
    

    public async Task<HashSeed> GetAndAppendHashSeed() {
        var seed = await GetHashSeed();
        var curSeed = new HashSeed {
            NextSeed = seed.NextSeed,
        };
        seed.NextSeed += 1;
        _context.HashSeeds.Update(seed);
        await _context.SaveChangesAsync();
        return curSeed;
    }
}