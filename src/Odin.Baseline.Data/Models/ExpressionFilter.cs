namespace Odin.Baseline.Data.Models
{
    public class ExpressionFilter
    {
        public string Field { get; set; }
        public ExpressionOperator Operator { get; set; }
        public object Value { get; set; }
    }
}
