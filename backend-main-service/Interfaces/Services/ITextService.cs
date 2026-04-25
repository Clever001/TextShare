using DocShareApi.ClassesLib;
using DocShareApi.Dtos.QueryOptions;
using DocShareApi.Dtos.QueryOptions.Filters;
using DocShareApi.Dtos.Text;
using DocShareApi.Models;

namespace DocShareApi.Interfaces.Services;

public interface ITextService {
    Task<Result<Text>> Create(string curUserName, CreateTextDto dto);
    Task<Result<Text>> GetById(string textId, string? curUserName, string? requestPassword);

    Task<Result<PaginatedResponseDto<Text>>> GetTexts(PaginationDto pagination, SortDto sort, TextFilterDto filter,
        string? senderName);

    Task<Result<PaginatedResponseDto<Text>>> GetTextsByName(PaginationDto pagination, SortDto sort,
        TextFilterWithoutOwnerDto filter, string ownerName, string? senderName);

    Task<Result<List<Text>>> GetLatestTexts();
    Task<Result<Text>> Update(string textId, string curUserName, UpdateTextDto dto);
    Task<Result> Delete(string textId, string curUserName);
}