namespace TextShareApi.Interfaces;

public interface IUniqueIdService {
    Task<string> GenerateNewHash();
}