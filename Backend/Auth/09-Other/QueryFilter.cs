using System.Linq.Expressions;
using Auth.Validator;
using Auth.CustomException;
using Microsoft.EntityFrameworkCore;
using Auth.Dto.Shared;


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
            PaginationPageDto.MIN_PAGE_NUMBER, 
            nameof(pageNumber)
        );
        BusinessLogicException.ThrowIfGreaterThan(
            pageNumber,
            PaginationPageDto.MAX_PAGE_NUMBER,
            nameof(pageNumber)
        );
        BusinessLogicException.ThrowIfLessThan(
            pageSize,
            PaginationPageDto.MIN_PAGE_SIZE,
            nameof(pageSize)
        );
        BusinessLogicException.ThrowIfGreaterThan(
            pageSize,
            PaginationPageDto.MAX_PAGE_SIZE,
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

    public async Task<(int countOfItems, IQueryable<ItemT> items)> 
    ApplyFilter(
        IQueryable<ItemT> items
    ) {
        items = ApplyWherePredicates(items);
        var countOfItems = await items.CountAsync();
        items = ApplySorting(items);
        items = ApplyPagination(items);

        return (countOfItems, items);
    }

    public IQueryable<ItemT> ApplyWherePredicates(IQueryable<ItemT> items) {
        if (ContainsWherePredicates) {
            foreach (var predicate in WherePredicates) {
                items = items.Where(predicate);
            }
        }
        return items;
    }

    public IQueryable<ItemT> ApplySorting(IQueryable<ItemT> items) {
        if (ContainsSortFilter) {
            if (ShouldSortAscending) {
                items = items.OrderBy(SortingKey);
            } else {
                items = items.OrderByDescending(SortingKey);
            }
        }
        return items;
    }

    public IQueryable<ItemT> ApplyPagination(IQueryable<ItemT> items) {
        items = items.Skip(SkipItemsCount);
        items = items.Take(TakeItemsCount);
        return items;
    }
}