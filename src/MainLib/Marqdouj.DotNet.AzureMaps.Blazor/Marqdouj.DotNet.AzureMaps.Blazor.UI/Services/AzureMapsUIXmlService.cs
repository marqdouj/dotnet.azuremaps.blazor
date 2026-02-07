using Marqdouj.DotNet.Web.Components.UI;
using Microsoft.Extensions.Logging;

namespace Marqdouj.DotNet.AzureMaps.Blazor.UI.Services
{
    public interface IAzureMapsUIXmlService : IXmlDocReader
    {
    }

    /// <summary>
    /// Provides XML documentation reading capabilities for the Azure Maps .NET assembly, enabling access to API
    /// metadata and documentation for Azure Maps-related types and members.
    /// </summary>
    /// <remarks>This service is intended for scenarios where programmatic access to XML documentation is
    /// required for Azure Maps APIs. Thread safety depends on the underlying logger and base class
    /// implementation.</remarks>
    /// <example>
    /// Add the service in Program.cs
    /// <code>
    /// builder.Services.AddScoped{IAzureMapsXmlService, AzureMapsXmlService}();
    /// </code>
    /// </example>
    /// <param name="logger">The logger used to record diagnostic and operational information for the service.</param>
    public sealed class AzureMapsUIXmlService(ILogger<AzureMapsUIXmlService>? logger) : XmlDocReader(assemblyName, logger), IAzureMapsUIXmlService
    {
        private const string assemblyName = "Marqdouj.DotNet.AzureMaps.Blazor";
    }
}
