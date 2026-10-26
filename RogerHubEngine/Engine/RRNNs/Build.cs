namespace RogerHubEngine.Engine.RRNNs
{
    public class Build
    {
        public BuildParameters bparams = new();
    }

    public class BuildParameters
    {
        public CompilationType Type;
    }

    /// <summary>
    /// Determines the build type.
    /// </summary>
    public enum CompilationType
    {
        /// <summary>
        /// Virtual build - for integrating neural networks into games or other projects, does not have its own body and cannot access anything.RRNNs Runtime ignores these builds.
        /// </summary>
        Virtual = 0,
        /// <summary>
        /// Physical build - for integrating neural networks into robots, can access physical board pins.
        /// </summary> 
        Physical = 1
    }
}
