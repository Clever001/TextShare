using System.Linq.Expressions;
using System.Reflection;

namespace TextShareApi.Mappers;

public static class SortExpressionBuilder {
    public static Expression<Func<T1, T2>> BuildSortExpression<T1, T2>(string propertyName) {
        if (string.IsNullOrWhiteSpace(propertyName)) {
            throw new ArgumentException("Property name cannot be null or empty.", nameof(propertyName));
        }
        Type type = typeof(T1);

        var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
        if (property == null) {
            throw new ArgumentException($"Property '{propertyName}' does not exist on type '{type.Name}'.");
        }

        ParameterExpression parameter = Expression.Parameter(type, "x");
        MemberExpression propertyAccess = Expression.Property(parameter, property);

        return Expression.Lambda<Func<T1, T2>>(propertyAccess, parameter);
    }
}