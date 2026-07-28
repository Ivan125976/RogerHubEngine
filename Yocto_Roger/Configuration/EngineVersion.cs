namespace Yocto_Roger.Configuration
{
    /// <summary>
    /// Contains immutable variables
    /// </summary>
    public class EngineVersion
    {
        /// <summary>
        /// Major version of RogerHubEngine
        /// </summary>
        public const char majorVersion = '2';

        /// <summary>
        /// Minor version of RogerHubEngine
        /// </summary>
        public const char minorVersion = '3';

        /// <summary>
        /// Patch version of RogerHubEngine
        /// </summary>
        public const char patchVersion = '0';

        /// <summary>
        /// Name of build
        /// </summary>
        public const Roadmap specialName = Roadmap.DEV;
    }

    /// <summary>
    /// Enum with name of builds
    /// </summary>
    public enum Roadmap
    {
        /// <summary>
        /// First stage: highly unstable development. It might not be working.
        /// </summary>
        DEV,

        /// <summary>
        /// Second stage: highly unstable implementation
        /// </summary>
        DEV2,

        /// <summary>
        /// Third stage: initial stabilization and optimization of new functionality.
        /// </summary>
        BETA,

        /// <summary>
        /// Fourth stage: subsystem improvements and rapid changes
        /// </summary>
        CHARLIE,

        /// <summary>
        /// Fifth stage: final adjustments and refinement of the neural network's connection with the subsystems.
        /// </summary>
        DELTA,

        /// <summary>
        /// possible builds based on stable versions with new functionality, without optimization
        /// </summary>
        EXPERIMENT
    }
}
