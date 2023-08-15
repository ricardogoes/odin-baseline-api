using Odin.Baseline.Data.Models;
using System.Linq.Expressions;
using System.Reflection;

namespace Odin.Baseline.Data.Helpers
{ 
    public static class ExpressionsHelper<T> where T : class
    {
        public static Expression<Func<T, bool>> BuildQueryableExpression(List<ExpressionFilter> filters)
        {

            var param = Expression.Parameter(typeof(T), "p");

            Expression body = null;

            foreach (var pair in filters)
            {
                var member = Expression.Property(param, pair.Field);
                var constant = Expression.Constant(pair.Value);

                Expression expression = null;                
                switch(pair.Operator)
                {                    
                    case ExpressionOperator.Equal:
                        expression = Expression.Equal(member, Expression.Convert(constant, member.Type));
                        break;
                    case ExpressionOperator.Contains:
                        var memberLowered = Expression.Call(member,
                            typeof(string).GetMethod("ToLower", Type.EmptyTypes));

                        var constantLowered = Expression.Constant(pair.Value.ToString().ToLower());

                        expression = Expression.Call(memberLowered, "Contains", Type.EmptyTypes, constantLowered);
                        break;
                    case ExpressionOperator.In:
                        var propertyType = ((PropertyInfo)member.Member).PropertyType;
                        expression = Expression.Call(typeof(Enumerable), "Contains", new[] { propertyType }, constant, member);
                        break;
                    case ExpressionOperator.GreaterThanOrEqual:
                        expression = Expression.GreaterThanOrEqual(member, Expression.Constant(constant));
                        break;
                    case ExpressionOperator.LessThanOrEqual:
                        Expression.GreaterThanOrEqual(member, Expression.Constant(constant));
                        break;
                };

                body = body == null ? expression : Expression.AndAlso(body, expression);
            }

            return ExpressionsCacheHelper<T>.GetPredicate(Expression.Lambda<Func<T, bool>>(body, param));
        }
    }
}
