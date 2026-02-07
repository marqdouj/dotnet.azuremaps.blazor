using System.Reflection;
using System.Text.Json.Serialization;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Common
{
    public class JSInteropDef : ICloneable
    {
        [JsonInclude]
        internal bool IsMarqdouj { get; } = true;

        /// <summary>
        /// Unique identifier for the map object.
        /// </summary>
        public string Id { get; set { if (string.IsNullOrWhiteSpace(value)) return; field = value.TrimCssId(); } } = AzMapsExtensions.GetRandomCssId();

        /// <summary>
        /// <see cref="ICloneable"/>
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public abstract class JsInteropBase
    {
        /// <summary>
        /// Unique identifier for the map object.
        /// </summary>
        public virtual string Id { get => JsInterop.Id; set => JsInterop.Id = value; }

        [JsonInclude]
        internal JSInteropDef JsInterop { get; set { ArgumentNullException.ThrowIfNull(field, nameof(JsInterop)); field = value; } } = new JSInteropDef();
    }

    /// <summary>
    /// Extension methods for objects that implement the JsInteropBase pattern.
    /// </summary>
    public static class JSInteropBaseExtensions
    {
        /// <summary>
        /// Resets the JavaScript interop identifier for the specified object and, optionally, its nested members.
        /// </summary>
        /// <remarks>This method is typically used to assign new unique identifiers to objects that
        /// participate in JavaScript interop scenarios, such as when cloning or reusing components. If deep is set to
        /// true, the method traverses public instance properties of the object and resets identifiers on any nested
        /// members that are of a compatible type.</remarks>
        /// <typeparam name="T">The type of the object whose JavaScript interop identifier is to be reset. Must be a reference type.</typeparam>
        /// <param name="obj">The object whose JavaScript interop identifier will be reset. If the object is null, no action is taken.</param>
        /// <param name="deep">true to recursively reset identifiers on nested members; otherwise, false. The default is true.</param>
        public static void ResetJsInteropId<T>(this T obj, bool deep = true) where T : class
        {
            if (obj is null) return;

            if (obj is JsInteropBase jsInteropBase)
            {
                jsInteropBase.JsInterop.Id = AzMapsExtensions.GetRandomCssId();
            }

            if (deep)
            {
                var properties = obj.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => !p.PropertyType.IsClass);

                foreach (var property in properties)
                {
                    var value = property.GetValue(obj);

                    if (value is not null)
                    {
                        if (value is JsInteropBase jsInteropValue)
                        {
                            jsInteropValue.ResetJsInteropId(true);
                        }
                        else if (value is IEnumerable<JsInteropBase> jsInteropCollection)
                        {
                            foreach (var item in jsInteropCollection)
                            {
                                item.ResetJsInteropId(true);
                            }
                        }
                    }
                }
            }
        }
    }
}
