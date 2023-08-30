using System.Linq.Expressions;

namespace Odin.Baseline.Infra.Data.EF.Expressions
{
    public static class ExpressionsCacheHelper<T> where T : class
    {
        private static readonly Dictionary<int, Expression<Func<T, bool>>> Cache = new();

        public static Expression<Func<T, bool>> GetPredicate(Expression<Func<T, bool>> expression)
        {
            var key = expression.GetHashCode();

            if (Cache.TryGetValue(key, out var cachedDelegate))
                return expression;

            Cache[key] = cachedDelegate;

            return expression;
        }
    }
}
