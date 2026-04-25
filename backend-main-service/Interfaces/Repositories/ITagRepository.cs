using System.Linq.Expressions;
using TextShareApi.Models;

namespace TextShareApi.Interfaces.Repositories;

public interface ITagRepository {
    public Task<List<Tag>> GetTags(Expression<Func<Tag, bool>> predicate);
}