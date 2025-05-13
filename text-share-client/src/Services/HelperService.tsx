export const getSyntaxes = () : string[] => {
    const syntaxes : string[] = [
        "csharp", "typescript", "javascript", "sql", "c++", "c", "java", "python",
        "rust", "pascal", "golang"
    ];
    return syntaxes.sort(((lhs, rhs) => lhs.localeCompare(rhs)))
};
