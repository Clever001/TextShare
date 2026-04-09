namespace Auth.Other;

public interface IPaginationFilter {
    int SkipItemsCount {get;}
    int TakeItemsCount {get;}
    int PageNumber {get;}
    int PageSize {get;}
}