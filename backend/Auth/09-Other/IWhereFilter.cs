using System.Linq.Expressions;

namespace Auth.Other;

public interface IWhereFilter<ItemT> {
    IEnumerable<Expression<Func<ItemT, bool>>> WherePredicates {get;}
}