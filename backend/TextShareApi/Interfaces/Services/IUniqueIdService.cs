namespace TextShareApi.Interfaces.Services;

public interface IUniqueIdService {
    Task<string> GenerateNewHash();
}