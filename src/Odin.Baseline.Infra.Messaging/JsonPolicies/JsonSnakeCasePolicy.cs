using Odin.Baseline.Infra.Messaging.Extensions;
using System.Text.Json;

namespace Odin.Baseline.Infra.Messaging.JsonPolicies
{
    public class JsonSnakeCasePolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
            => name.ToSnakeCase();
    }
}
