using System.Linq.Expressions;
using System.Reflection;

namespace Odin.Baseline.Infra.Data.EF.Expressions
{
    public static class ExpressionsFactory<T> where T : class
    {
        public static Expression<Func<T, bool>> BuildQueryableExpression(List<ExpressionFilter> filters)
        {
            if (filters is null)
                return null;

            var param = Expression.Parameter(typeof(T), "p");

            Expression body = null;

            foreach (var pair in filters)
            {
                var member = Expression.Property(param, pair.Field);
                var constant = Expression.Constant(pair.Value);

                Expression expression = null;
                switch (pair.Operator)
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
                        expression = Expression.GreaterThanOrEqual(member, Expression.Convert(constant, member.Type));
                        break;
                    case ExpressionOperator.LessThanOrEqual:
                        if(member.Type == typeof(DateTime))
                        {
                            var endOfDayConstant = Expression.Constant(((DateTime)constant.Value).EndOfDay());
                            expression = Expression.LessThanOrEqual(member, Expression.Convert(endOfDayConstant, member.Type));
                        }
                        
                        break;
                };

                body = body == null ? expression : Expression.AndAlso(body, expression);
            }

            return ExpressionsCacheHelper<T>.GetPredicate(Expression.Lambda<Func<T, bool>>(body, param));
        }

        public static List<ExpressionFilter> BuildFilterExpression(Dictionary<string, object> filters)
        {
            var expressionsFilter = new List<ExpressionFilter>();

            foreach (var key in filters.Keys)
            {
                if (!string.IsNullOrWhiteSpace(filters[key]?.ToString()) && filters[key] is string)
                    expressionsFilter.Add(new ExpressionFilter { Field = key, Operator = ExpressionOperator.Contains, Value = filters[key] });
                
                else if (!string.IsNullOrWhiteSpace(filters[key]?.ToString()) && filters[key] is bool)
                    expressionsFilter.Add(new ExpressionFilter { Field = key, Operator = ExpressionOperator.Equal, Value = filters[key] });
                
                else if (!string.IsNullOrWhiteSpace(filters[key]?.ToString()) && filters[key] is Guid && Guid.Parse(filters[key].ToString()) != Guid.Empty)
                    expressionsFilter.Add(new ExpressionFilter { Field = key, Operator = ExpressionOperator.Equal, Value = filters[key] });

                else if (!string.IsNullOrWhiteSpace(filters[key]?.ToString()) && 
                    filters[key] is DateTime && 
                    DateTime.Parse(filters[key].ToString()) != default &&
                    key.Contains("Start"))
                    expressionsFilter.Add(new ExpressionFilter { Field = key.Replace("Start", ""), Operator = ExpressionOperator.GreaterThanOrEqual, Value = filters[key] });

                else if (!string.IsNullOrWhiteSpace(filters[key]?.ToString()) &&
                    filters[key] is DateTime &&
                    DateTime.Parse(filters[key].ToString()) != default &&
                    key.Contains("End"))
                    expressionsFilter.Add(new ExpressionFilter { Field = key.Replace("End", ""), Operator = ExpressionOperator.LessThanOrEqual, Value = filters[key] });
            }

            return expressionsFilter.Any() ? expressionsFilter : null;
        }
    }

    public class ExpressionFilter
    {
        public string Field { get; set; }
        public ExpressionOperator Operator { get; set; }
        public object Value { get; set; }
    }

    public enum ExpressionOperator
    {
        Contains,
        Equal,
        GreaterThanOrEqual,
        In,
        LessThanOrEqual
    }
}
