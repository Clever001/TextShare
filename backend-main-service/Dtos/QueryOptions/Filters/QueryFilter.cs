using System.Linq.Expressions;

namespace DocShareApi.Dtos.QueryOptions.Filters;

public record QueryFilter<SearchT, OrderT>(
    int Skip,
    int Take,
    Expression<Func<SearchT, OrderT>> KeyOrder,
    bool IsAscending,
    List<Expression<Func<SearchT, bool>>>? Predicates
);