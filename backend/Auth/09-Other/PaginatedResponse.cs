namespace Auth.Other;

public sealed class PaginatedResponse<T> {
    private readonly IEnumerable<T> itemsSelection;
    private readonly int totalItemsCount;
    private readonly int currentPage;
    private readonly int pageSize;

    public PaginatedResponse(
        IEnumerable<T> itemsSelection,
        int totalItemsCount,
        int currentPage,
        int pageSize
    ) {
        this.itemsSelection = itemsSelection.ToArray();
        this.totalItemsCount = totalItemsCount;
        this.currentPage = currentPage;
        this.pageSize = pageSize;
    }

    public IEnumerable<T> Items {
        get => itemsSelection;
    }
    public int TotalItems {
        get => totalItemsCount;
    }
    public int TotalPages {
        get => (int)Math.Ceiling(TotalItems / (double)PageSize);
    }
    public int CurrentPage {
        get => currentPage;
    }
    public int PageSize {
        get => pageSize;
    }
}