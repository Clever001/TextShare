using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Text;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface ITextService {
    public Task<Result<Text>> Create(string curUserName);
    public Task<Result<Text>> GetById(string id, string? curUserName, string? requestPassword);
    public Task<Result<List<Text>>> GetAccountTexts(string curUserName);
    public Task<Result<List<Text>>> GetAllAvailable(string? curUserName);
    public Task<Result<Text>> Update(string id, UpdateTextDto dto);
    public Task<Result> Delete(string id);
}