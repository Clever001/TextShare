using System.Buffers.Binary;
using System.Buffers.Text;
using TextShareApi.Interfaces.Repositories;
using TextShareApi.Interfaces.Services;

namespace TextShareApi.Services;

public class UniqueIdService : IUniqueIdService {
    private readonly IHashSeedRepository _repo;
    public UniqueIdService(IHashSeedRepository repo) {
        _repo = repo;
    }
    
    public async Task<string> GenerateNewHash() {
        var seed = await _repo.GetAndAppendHashSeed();
        
        Span<byte> byteArray = stackalloc byte[8];
        BinaryPrimitives.WriteUInt64LittleEndian(byteArray, seed.NextSeed);
        return Base64Url.EncodeToString(byteArray);
    }
}