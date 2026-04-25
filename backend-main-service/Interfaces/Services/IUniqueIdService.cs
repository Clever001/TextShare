namespace DocShareApi.Interfaces.Services;

public interface IUniqueIdService {
    Task<string> GenerateNewHash();
}