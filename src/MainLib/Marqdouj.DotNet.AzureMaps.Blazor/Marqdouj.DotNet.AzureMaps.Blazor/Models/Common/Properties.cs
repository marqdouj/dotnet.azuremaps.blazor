using System.Text.Json;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Common
{
    public class Properties : Dictionary<string, object> 
    {
        public override string ToString() => this.Any() ? JsonSerializer.Serialize(this) : "";
    }
}
