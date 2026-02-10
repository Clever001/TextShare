using Auth.Other;

namespace Auth.Dto.Shared;

public record PaginationPageDto(
    int PageNumber,
    int PageSize
) : ICheckable {

    public static int MIN_PAGE_NUMBER { get; } = 1;
    public static int MAX_PAGE_NUMBER { get; } = 100_000;
    public static int MIN_PAGE_SIZE { get; } = 1;
    public static int MAX_PAGE_SIZE { get; } = 50;

    public virtual DtoChecker.DtoCheckResult CheckValidity() {
        var dtoChecker = new DtoChecker();

        dtoChecker.AddErrorIfValueIsLessThan(PageNumber, MIN_PAGE_NUMBER, nameof(PageNumber));
        dtoChecker.AddErrorIfValueIsGreaterThan(PageNumber, MAX_PAGE_NUMBER, nameof(PageNumber));
        dtoChecker.AddErrorIfValueIsLessThan(PageSize, MIN_PAGE_SIZE, nameof(PageSize));
        dtoChecker.AddErrorIfValueIsGreaterThan(PageSize, MAX_PAGE_SIZE, nameof(PageSize));

        return dtoChecker.GetCheckResult();
    }
}
