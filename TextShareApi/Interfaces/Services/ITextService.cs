using TextShareApi.ClassesLib;
using TextShareApi.Dtos.Text;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface ITextService {
    Task<Result<Text>> Create(string curUserName);
    Task<Result<Text>> GetById(string textId, string? curUserName, string? requestPassword);
    Task<Result<List<Text>>> GetAccountTexts(string curUserName);
    Task<Result<List<Text>>> GetAllAvailable(string? curUserName);
    Task<Result<Text>> Update(string textId, string curUserName, string? requestPassword, UpdateTextDto dto);
    Task<Result> Delete(string textId, string curUserName, string? requestPassword);

    Task<Result> Contains(string textId);
}