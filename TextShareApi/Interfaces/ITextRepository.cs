using TextShareApi.Models;

namespace TextShareApi.Interfaces;

public interface ITextRepository {
    Task<Text> CreateText(Text text);
    Task<Text?> GetTextById(string textId);
}