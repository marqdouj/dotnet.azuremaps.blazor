using System.Runtime.CompilerServices;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop.Modules
{
    internal enum JsModule
    {
        MapFactory,
        Animations,
        Configurations,
        Controls,
        Events,
        Features,
        Geolocations,
        ImageSprites,
        Layers,
        Markers,
        Mercators,
        Popups,
        Sources,
    }

    internal static class ModuleExtensions
    {
        internal const string LIBRARY_NAME = "marqdoujAzureMapsBlazor";

        private static string GetNamespace(this JsModule jsModule)
        {
            return jsModule.ToString().Replace("_", ".");
        }

        internal static string GetJsModuleMethod(JsModule module, [CallerMemberName] string name = "")
            => $"{LIBRARY_NAME}.{module.GetNamespace()}.{name.ToJsonName()}";

        /// <summary>
        /// first char must be lowercase
        /// </summary>
        internal static string ToJsonName(this string name)
        {
            var firstChar = name[0].ToString().ToLower();
            var remainder = name.Substring(1);
            return $"{firstChar}{remainder}";
        }

        /// <summary>
        /// first char must be lowercase
        /// </summary>
        internal static string ToJsonName<T>(this T value) where T : Enum
        {
            return ToJsonName(value.ToString());
        }
    }
}
