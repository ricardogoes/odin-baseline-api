public static partial class Extensions
{
    public static DateTime EndOfDay(this DateTime @this)
    {
        return new DateTime(@this.Year, @this.Month, @this.Day).AddDays(1).Subtract(new TimeSpan(0, 0, 0, 0, 1));
    }
}