namespace Auth.Other;

public record SelectionOfItems<T>(
    int TotalCount,
    T[] Selection
);