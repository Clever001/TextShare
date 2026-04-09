using System.Linq.Expressions;

namespace Auth.Other;

public interface ISortFilter<ItemT, KeyT> {
    Expression<Func<ItemT, KeyT>> SortingKey {get;}
    bool ShouldSortAscending {get;}
}