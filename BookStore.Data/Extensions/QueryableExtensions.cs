using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace BookStore.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class QueryableExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
                if (seenKeys.Add(keySelector(element)))
                    yield return element;
        }

        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, string key,
            bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(key))
                return query;

            var lambda = (dynamic)CreateExpression(typeof(TSource), key);

            return ascending
                ? Queryable.OrderBy(query, lambda)
                : Queryable.OrderByDescending(query, lambda);
        }

        private static LambdaExpression CreateExpression(Type type, string propertyName)
        {
            var param = Expression.Parameter(type, "x");

            Expression body = param;
            foreach (var member in propertyName.Split('.'))
                body = Expression.PropertyOrField(body, member);

            return Expression.Lambda(body, param);
        }
    }
}
