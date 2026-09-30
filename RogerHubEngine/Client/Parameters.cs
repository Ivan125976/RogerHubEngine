namespace RogerHubEngine.Client
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
RogerHub configuration
*/

    /// <summary>
    /// contains all RogerHubEngine parameters
    /// </summary>
    public static class Parameters
    {
        /// <summary>
        /// Number of passes during training on training data
        /// </summary>
        public static int passes = 10000;

        /// <summary>
        /// The coefficient of change of weights and biases of the neural network.
        /// </summary>
        public static float learningRate = 0.01f;

        /// <summary>
        /// The percentage of response of the built-in DropOut subsystem during neural network training
        /// </summary>
        public static float DropOutPercent = 8.0f;

        /// <summary>
        /// The path to the neural network's knowledge base on which it will need to be trained
        /// </summary>
        public static string knowledgeFile = string.Empty;

        /// <summary>
        /// The path to the finished neural network, sealed in a file
        /// </summary>
        public static string roger2 = string.Empty;

        /// <summary>
        /// Number of input neurons
        /// </summary>
        public static int inputNeuronsCount = 8;

        /// <summary>
        /// Number of middle neurons
        /// </summary>
        public static int middleNeuronsCount = 12;

        /// <summary>
        /// Number of output neurons
        /// </summary>
        public static int outputNeuronsCount = 8;

        /// <summary>
        /// Number of layers in a neural network
        /// </summary>
        public static int layers = 4;

        /// <summary>
        /// Cache Management in RMS Optimization
        /// </summary>
        public static float rms_decay = 0.95f;

        /// <summary>
        /// RMS Optimization Switch
        /// </summary>
        public static bool rms_enabled = false;
    }
}
