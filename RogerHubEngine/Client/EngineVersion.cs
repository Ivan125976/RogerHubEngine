/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

namespace RogerHubEngine.Client
{
    /// <summary>
    /// Contains immutable variables
    /// </summary>
    public class EngineVersion
    {
        /// <summary>
        /// Version of RogerHubEngine
        /// </summary>
        public static readonly Version engineVersion = new(2, 3, 0);

        /// <summary>
        /// Version of Robot Realtime Neural Network system Compilator
        /// </summary>
        public static readonly Version RRNNSCompilatorVersion = new(0, 0, 1);

        /// <summary>
        /// Name of build
        /// </summary>
        internal const Roadmap specialName = Roadmap.D2;
    }

    internal enum Roadmap
    {
        D, //DEV
        D2, //DEV2
        BT, //BETA
        CRL, //CHARLIE
        DLT, //DELTA
        EXP, //EXPERIMENT
        R //RELEASE
    }
}
