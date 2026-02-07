using Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Events
{
    public enum MapsManagerError
    {
        None,
        AuthenticationFailed,
        CreateInstanceFailed,
    }

    public class MapsManagerEventArgs
    {
        internal MapsManagerEventArgs(IAzMapsManager? manager)
        {
            Manager = manager;
        }

        internal MapsManagerEventArgs(MapsManagerError error, Exception? exception)
        {
            Error = error;
            Exception = exception;
        }

        public IAzMapsManager? Manager { get; }

        public bool Success => Manager is not null;

        public MapsManagerError Error { get; set; }

        public Exception? Exception { get; }

        public string ExceptionMessage
        {
            get
            {
                return Error switch
                {
                    MapsManagerError.AuthenticationFailed => $"MapsManager authentication failed. {Exception?.Message}",
                    _ => $"MapsManager creation failed. {Exception?.Message}",
                };
            }
        }
    }
}
