using System.Buffers.Binary;
using System.Buffers.Text;
using System.Text.Json;
using TextShareApi.Models;

namespace TextShareApi.Services;

public class HashGenerator {
    public static async Task<HashSeed?> LoadSeed() {
        try {
            if (!File.Exists("hashSeed.json")) {
                return null;
            }

            Stream fileStream = File.OpenRead("hashSeed.json");
            return await JsonSerializer.DeserializeAsync<HashSeed>(fileStream);
        }
        catch (Exception e) {
            return null;
        }
    }

    public static Task SaveAsync(HashSeed seed) {
        return Task.Run(() => {
            string json = JsonSerializer.Serialize(seed);
            File.WriteAllText("hashSeed.json", json);
        });
    }

    public static Task<string> GenerateHash(UInt64 seed) {
        return Task.Run(() => {
            Span<byte> byteArray = stackalloc byte[8];
            BinaryPrimitives.WriteUInt64LittleEndian(byteArray, seed);
            return Base64Url.EncodeToString(byteArray);
        });
    }
}