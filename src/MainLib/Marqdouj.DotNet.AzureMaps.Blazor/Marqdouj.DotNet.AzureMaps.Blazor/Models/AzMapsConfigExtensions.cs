using Marqdouj.DotNet.AzureMaps.Blazor.Models.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models
{
    /// <summary>
    /// Extension method for AzMaps configuration.
    /// </summary>
    public static class AzMapsConfigExtensions
    {
        /// <summary>
        /// Configures <see cref="MapConfiguration"/>, optionally enabling configuration validation.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="config">A delegate that configures the <see cref="MapConfiguration"/> instance used for Azure Maps integration.</param>
        /// <param name="validate">Specifies whether to enable validation of the <see cref="MapConfiguration"/> during configuration. 
        /// Set to <see langword="true"/> to validate the configuration; otherwise, <see langword="false"/> to validate in the Component.</param>
        /// <returns>An instance of the resolved <see cref="MapConfiguration"/></returns>
        public static MapConfiguration AddMarqdoujAzureMapsBlazor(this IServiceCollection services, Action<MapConfiguration> config, bool validate = false)
        {
            if (validate)
            {
                services
                    .AddOptions<MapConfiguration>()
                    .Configure(config)
                    .Validate(c => c.Authentication.IsValid(), config.InValidMessage());
            }
            else
            {
                services
                    .AddOptions<MapConfiguration>()
                    .Configure(config);
            }

            return config.GetConfiguration();
        }

        private static string InValidMessage(this Action<MapConfiguration> config)
        {
            var c = config.GetConfiguration();
            return c.Authentication.InValidMessage();
        }

        private static MapConfiguration GetConfiguration(this Action<MapConfiguration> config)
        {
            var c = new MapConfiguration();
            config.Invoke(c);
            return c;
        }
    }
}
