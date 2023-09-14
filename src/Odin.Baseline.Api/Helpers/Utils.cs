using Odin.Baseline.Infra.Messaging.Extensions;

namespace Odin.Baseline.Api.Helpers
{
    public class Utils
    {
        public static string? GetSortParam(string? sort)
        {
            if (string.IsNullOrWhiteSpace(sort))
                return null;

            if (sort.Contains(" "))
            {
                var splittedSort = sort.Split(' ');
                return $"{splittedSort[0].ToPascalCase()} {splittedSort[1]}";
            }
            else
                return sort;
        }
    }
}
