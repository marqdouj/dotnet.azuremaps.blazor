using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models
{
    internal static class AzMapsExtensions
    {
        /// <summary>
        /// Provides default options for JSON serialization, including camel case property naming and ignoring null
        /// values when writing JSON.
        /// </summary>
        /// <remarks>These options can be used to ensure consistent JSON output across the application.
        /// The property naming policy converts property names to camel case, and properties with null values are
        /// omitted from the serialized JSON.</remarks>
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Serializes the specified object to a JSON string suitable for JavaScript interop scenarios.
        /// For some Azure Maps Web SDK settings, <see langword="null"/> is treated differently than JavaScript 'undefined'.
        /// </summary>
        /// <remarks>The resulting JSON is intended for use with JavaScript interop, such as when passing
        /// data between .NET and JavaScript in Blazor applications. The serialization behavior depends on the
        /// configured serializer options and may not include all .NET-specific types or features.</remarks>
        /// <param name="model">The object to serialize. Can be any serializable .NET object.</param>
        /// <param name="options">JsonSerializerOptions. Default is to ignore null properties. <see cref="serializerOptions"/></param>
        /// <returns>A JSON-formatted string representation of the specified object. 
        /// Some Azure Maps SDK configurations treat <see langword="null"/> differently than JS 'undefined'.
        /// Removing the <see langword="null"/> properties from the JSON resolves the issue.</returns>
        public static string SerializeForJsInterop(this object model, JsonSerializerOptions? options = null)
        {
            var json = JsonSerializer.Serialize(model, options ?? serializerOptions);
            return json;
        }

        internal static string EnumToJson<T>(this T value, string underscoreReplacement = "-") where T : Enum
        {
            return value.ToString().ToLower().Replace("_", underscoreReplacement);
        }

        internal static List<string> EnumToJson<T>(this IEnumerable<T>? items, string underscoreReplacement = "-") where T : Enum
            => items?.Select(e => e.EnumToJson(underscoreReplacement)).Distinct().OrderBy(e => e).ToList() ?? [];

        internal static List<T> JsonToEnum<T>(this IEnumerable<string>? items) where T : Enum
        {
            var toProcess = items?.Where(e => !string.IsNullOrWhiteSpace(e)).Distinct().ToList();
            var values = toProcess?.Select(e => e.JsonToEnum<T>()).ToList() ?? [];
            return [.. values.Select(e => e)];
        }

        internal static T JsonToEnum<T>(this string? value, string hyphenReplacement = "_", T defaultValue = default!) where T : Enum
        {
            value = value?.Replace("-", hyphenReplacement);
            return Enum.TryParse(typeof(T), value, true, out var result) ? (T)result : defaultValue;
        }

        internal static string? EnumToJsonN<T>(this T? value, string underscoreReplacement = "-") where T : struct, Enum
        {
            return value?.ToString().ToLower().Replace("_", underscoreReplacement);
        }

        internal static T? JsonToEnumN<T>(this string? value, string hyphenReplacement = "_", T? defaultValue = (T?)null) where T : struct, Enum
        {
            value = value?.Replace("-", hyphenReplacement);
            return Enum.TryParse(typeof(T), value, true, out var result) ? (T)result : defaultValue;
        }

        internal static string GetRandomCssId()
        {
            return $"g_{Guid.NewGuid()}";
        }

        internal static void EnsureCount(this List<double> items, int min, int? max = null, double addDefault = 0)
        {
            while (items.Count < min)
            {
                items.Add(addDefault);
            }

            //Remove excess values
            if (max != null)
            {
                while (items.Count > max)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }
        }

        internal static void EnsureCount(this List<Position> items, int min, int? max = null)
        {
            while (items.Count < min)
            {
                items.Add(new Position(0, 0));
            }

            //Remove excess values
            if (max != null)
            {
                while (items.Count > max)
                {
                    items.RemoveAt(items.Count - 1);
                }
            }
        }

        internal static string TrimCssId(this string value)
        {
            return value.Trim().Replace(" ", "_");
        }
    }
}
