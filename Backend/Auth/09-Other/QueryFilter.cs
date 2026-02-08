using System.Linq.Expressions;
using Auth.Validator;
using Auth.CustomException;


namespace Auth.Other;

public class QueryFilter<ItemT, KeyT>
: IPaginationFilter, ISortFilter<ItemT, KeyT>, IWhereFilter<ItemT> {
    private readonly int pageNumber;
    private readonly int pageSize;
    private readonly Expression<Func<ItemT, KeyT>>? keyOrder;
    private readonly bool isAscending;
    private readonly IEnumerable<Expression<Func<ItemT, bool>>> predicates;

    internal QueryFilter(
        int pageNumber, int pageSize,
        Expression<Func<ItemT, KeyT>>? keyOrder, bool isAscending,
        IEnumerable<Expression<Func<ItemT, bool>>> predicates
    ) {
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.keyOrder = keyOrder;
        this.isAscending = isAscending;
        this.predicates = predicates.ToArray();

        CheckPaginationInit();
    }

    private void CheckPaginationInit() {
        BusinessLogicException.ThrowIfLessThan(
            pageNumber, 
            PaginationPageValidator.MIN_PAGE_NUMBER, 
            nameof(pageNumber)
        );
        BusinessLogicException.ThrowIfGreaterThan(
            pageNumber,
            PaginationPageValidator.MAX_PAGE_NUMBER,
            nameof(pageNumber)
        );
        BusinessLogicException.ThrowIfLessThan(
            pageSize,
            PaginationPageValidator.MIN_PAGE_SIZE,
            nameof(pageSize)
        );
        BusinessLogicException.ThrowIfGreaterThan(
            pageSize,
            PaginationPageValidator.MAX_PAGE_SIZE,
            nameof(pageSize)
        );
    }

    public int SkipItemsCount {
        get => (pageNumber - 1) * pageSize;
    }

    public int TakeItemsCount {
        get => pageSize;
    }

    public int PageNumber {
        get => pageNumber;
    }
    
    public int PageSize {
        get => pageSize;
    }

    public bool ContainsPaginationFilter {
        get => true; // Api should always include pagination.
    }

    public Expression<Func<ItemT, KeyT>> SortingKey {
        get {
            if (!ContainsSortFilter) {
                throw new InvalidOperationException("Key order is not inited");
            }
            return keyOrder!;
        }
    }

    public bool ShouldSortAscending {
        get {
            if (!ContainsSortFilter) {
                throw new InvalidOperationException("Key order is not inited");
            }
            return isAscending;
        }
    }

    public bool ContainsSortFilter {
        get => keyOrder != null;
    }

    public IEnumerable<Expression<Func<ItemT, bool>>> WherePredicates {
        get => predicates;
    }

    public bool ContainsWherePredicates {
        get => predicates.Any();
    }
}