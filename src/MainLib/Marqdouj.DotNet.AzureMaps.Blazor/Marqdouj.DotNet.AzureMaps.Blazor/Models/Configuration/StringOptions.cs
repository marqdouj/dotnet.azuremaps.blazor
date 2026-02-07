namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration
{
    public class StringOptions : List<string>
    {
        public override string ToString()
        {
            return string.Join(",", this);
        }
    }
}
