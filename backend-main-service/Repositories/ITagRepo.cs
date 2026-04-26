using System.Linq.Expressions;
using DocShareApi.Models;

namespace DocShareApi.Repositories;

public interface ITagRepo {
    Task<List<Tag>> GetTags(Expression<Func<Tag, bool>> predicate);
    Task CreateTags(List<Tag> tags);
    Task RemoveTags(List<Tag> tags);
}