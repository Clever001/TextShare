using System.Linq.Expressions;
using Auth.Dto.Shared;
using Auth.CustomException;


namespace Auth.Other;

public class QueryFilterBuilder<ItemT, KeyT> {
    private int? pageNumber;
    private int? pageSize;
    private Expression<Func<ItemT, KeyT>>? keyOrder;
    private bool? isAscending;
    private readonly List<Expression<Func<ItemT, bool>>> predicates = new();

    public QueryFilterBuilder<ItemT, KeyT> 
    WithPagination(PaginationPageDto paginationPage) {
        this.pageNumber = paginationPage.PageNumber;
        this.pageSize = paginationPage.PageSize;
        return this;
    }

    public QueryFilterBuilder<ItemT, KeyT> 
    WithSort(
        Expression<Func<ItemT, KeyT>> keyOrder,
        bool isAscending
    ) {
        this.keyOrder = keyOrder;
        this.isAscending = isAscending;
        return this;
    }

    public QueryFilterBuilder<ItemT, KeyT> 
    WithWherePredicate(Expression<Func<ItemT, bool>> predicate) {
        predicates.Add(predicate);
        return this;
    }

    public QueryFilterBuilder<ItemT, KeyT> 
    WithWherePredicates(params Expression<Func<ItemT, bool>>[] predicates) {
        return this.WithWherePredicates(
            predicates.AsEnumerable()
        );
    }

    public QueryFilterBuilder<ItemT, KeyT> 
    WithWherePredicates(IEnumerable<Expression<Func<ItemT, bool>>> predicates) {
        this.predicates.AddRange(predicates);
        return this;
    }

    public QueryFilter<ItemT, KeyT> Build() {
        CheckPaginationInit();
        CheckPredicatesInit();
        
        return new QueryFilter<ItemT, KeyT>(
            pageNumber!.Value,
            pageSize!.Value,
            keyOrder,
            isAscending ?? false,
            predicates
        );
    }

    private void CheckPaginationInit() {
        if (AnyIsNull([pageNumber, pageSize])) {
            throw new BusinessLogicException(
                $"{nameof(QueryFilterBuilder<,>)}: Pagination is not initialized"
            );
        }
    }

    private void CheckPredicatesInit() {
        if (AnyIsNull(predicates)) {
            throw new BusinessLogicException(
                $"{nameof(QueryFilterBuilder<,>)}: One or many of WherePredicates is not initialized"
            );
        }
    }

    private bool AnyIsNull(IEnumerable<object?> checkableObjects) {
        return checkableObjects?.Any(o => o == null) ?? true;
    }
}