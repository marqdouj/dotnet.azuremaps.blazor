using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sandbox.Components.Pages.AzureMaps
{
    internal static class Extensions
    {
        private static readonly JsonSerializerOptions jsonMinOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        internal static string ToJsonMin<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, jsonMinOptions);
        }
    }
}
