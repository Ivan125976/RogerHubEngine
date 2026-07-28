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
        public const char minorVersion = '2';

        /// <summary>
        /// Patch version of RogerHubEngine
        /// </summary>
        public const char patchVersion = '2';

        /// <summary>
        /// Name of build
        /// </summary>
        public const Roadmap specialName = Roadmap.R;
    }

    /// <summary>
    /// Enum with name of builds
    /// </summary>
    public enum Roadmap
    {
        /// <summary>
        /// First stage: highly unstable development. It might not be working.
        /// </summary>
        D,

        /// <summary>
        /// Second stage: highly unstable implementation
        /// </summary>
        D2,

        /// <summary>
        /// Third stage: initial stabilization and optimization of new functionality.
        /// </summary>
        BT,

        /// <summary>
        /// Fourth stage: subsystem improvements and rapid changes
        /// </summary>
        CRL,

        /// <summary>
        /// Fifth stage: final adjustments and refinement of the neural network's connection with the subsystems.
        /// </summary>
        DLT,

        /// <summary>
        /// possible builds based on stable versions with new functionality, without optimization
        /// </summary>
        EXP,

        /// <summary>
        /// Release
        /// </summary>
        R
    }
}
