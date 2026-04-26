namespace DocShareApi.Services;

public interface IUniqueIdService {
    Task<string> GenerateNewId();
}
