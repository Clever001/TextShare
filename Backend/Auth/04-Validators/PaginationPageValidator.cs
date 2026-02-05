using Auth.Dto.Shared;

namespace Auth.Validator;

public class PaginationPageValidator : Validator {
    public static int MIN_PAGE_NUMBER { get; } = 1;
    public static int MAX_PAGE_NUMBER { get; } = 100_000;
    public static int MIN_PAGE_SIZE { get; } = 1;
    public static int MAX_PAGE_SIZE { get; } = 50;

    private readonly PaginationPageDto checkablePage;

    public PaginationPageValidator(
        PaginationPageDto page
    ) : base() {
        checkablePage = page;
    }

    public override void PerformValidityCheck() {
        isValid = true;
        SetInvalidIfValueIsNotInRange(
            checkablePage.PageNumber,
            MIN_PAGE_NUMBER,
            MAX_PAGE_NUMBER,
            nameof(checkablePage.PageNumber)
        );
        SetInvalidIfValueIsNotInRange(
            checkablePage.PageSize,
            MIN_PAGE_SIZE,
            MAX_PAGE_SIZE,
            nameof(checkablePage.PageNumber)
        );
    }
}