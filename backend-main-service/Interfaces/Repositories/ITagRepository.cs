using System.Linq.Expressions;
using DocShareApi.Models;

namespace DocShareApi.Interfaces.Repositories;

public interface ITagRepository {
    public Task<List<Tag>> GetTags(Expression<Func<Tag, bool>> predicate);
}