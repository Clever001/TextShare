using TextShareApi.ClassesLib;
using TextShareApi.Dtos.QueryOptions;
using TextShareApi.Dtos.QueryOptions.Filters;
using TextShareApi.Dtos.Text;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Services;

public interface ITextService {
    Task<Result<Text>> Create(string curUserName, CreateTextDto dto);
    Task<Result<Text>> GetById(string textId, string? curUserName, string? requestPassword);
    Task<Result<PaginatedResponseDto<Text>>> GetTexts(PaginationDto pagination, SortDto sort, TextFilterDto filter, string? senderName);
    Task<Result<List<Text>>> GetLatestTexts();
    Task<Result<Text>> Update(string textId, string curUserName, UpdateTextDto dto);
    Task<Result> Delete(string textId, string curUserName);
}