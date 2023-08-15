using System.Linq.Expressions;

namespace Odin.Baseline.Data.Helpers
{
    public static class ExpressionsCacheHelper<T> where T : class
    {
        private static readonly Dictionary<int, Expression<Func<T, bool>>> Cache = new();
        public static Expression<Func<T, bool>> GetPredicate<T>(Expression<Func<T, bool>> expression)
        {
            var key = expression.GetHashCode();
            if (Cache.TryGetValue(key, out var cachedDelegate))
            {
                return expression;
            }
            Cache[key] = cachedDelegate;
            return expression;
        }
    }
}
