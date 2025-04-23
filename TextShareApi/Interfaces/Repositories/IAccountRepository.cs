namespace TextShareApi.Interfaces.Services;

public interface IAccountRepository {
    // TODO: Дополни репозиторий методами.
    Task<string?> GetAccountId(string userName);
}