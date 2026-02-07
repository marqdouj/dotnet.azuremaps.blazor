namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Animations
{
    /// <summary>
    /// Animation Action supported by this library.
    /// </summary>
    public enum AnimationAction
    {
        /// <summary>
        /// Animates the update of coordinates on a shape or HtmlMarker. 
        /// Shapes will stay the same type. 
        /// Only base animation options supported for geometries other than Point.
        /// </summary>
        SetCoordinates,
    }
}
