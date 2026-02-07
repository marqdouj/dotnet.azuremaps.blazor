using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;

namespace Marqdouj.DotNet.AzureMaps.Blazor.Models.Animations
{
    /// <summary>
    /// Provides configuration options for animating a map feature, including the target shape, associated data source,
    /// animation action, and animation settings.
    /// </summary>
    /// <remarks>Use this class to specify how a map feature should be animated, including which feature to
    /// animate, the data source it belongs to, the type of animation action to perform, and animation parameters such
    /// as easing and duration. The class is typically used when initiating or customizing shape animations in mapping
    /// applications.</remarks>
    public class ShapeAnimationOptions
    {
        /// <summary>
        /// Initializes a new instance of the ShapeAnimationOptions class with the specified feature, data source
        /// identifier, animation action, and easing function.
        /// </summary>
        /// <param name="feature">The map feature definition to be animated. Cannot be null.</param>
        /// <param name="dataSourceId">The identifier of the data source associated with the feature. Cannot be null or empty.</param>
        /// <param name="action">The animation action to perform on the feature. Defaults to AnimationAction.SetCoordinates.</param>
        /// <param name="easing">The easing function to use for the animation. Defaults to AnimationEasing.linear.</param>
        public ShapeAnimationOptions(
            MapFeatureDef feature,
            string dataSourceId,
            AnimationAction action = AnimationAction.SetCoordinates,
            AnimationEasing easing = AnimationEasing.linear)
        {
            Shape = feature;
            DataSourceId = dataSourceId;
            Action = action.ToString();
            AnimationOptions = new PlayableAnimationOptions { AutoPlay = true, Easing = easing.ToString(), Duration = 1500 };
        }

        /// <summary>
        /// <see cref="AnimationAction"/>
        /// </summary>
        public string Action { get; }

        /// <summary>
        /// The shape to animate.
        /// </summary>
        public MapFeatureDef Shape { get; }

        /// <summary>
        /// DataSource Id that contains the Shape.
        /// </summary>
        public string DataSourceId { get; set; }

        /// <summary>
        /// <see cref="AnimationOptions"/>
        /// </summary>
        public PlayableAnimationOptions AnimationOptions { get; set; }
    }
}
