using MemoryPack;

namespace Yocto_Roger.RogerCore
{
    /// <summary>
    /// Keeping values in string's
    /// </summary>
    [MemoryPackable]
    public partial class NeuralNetworkState
    {
        /// <summary>
        /// Input neurons
        /// </summary>
        public int[]? InputNeurons { get; set; }
        /// <summary>
        /// middle neurons
        /// </summary>
        public double[,]? MiddleNeurons { get; set; }
        /// <summary>
        /// output neurons
        /// </summary>
        public double[]? OutputNeurons { get; set; }
        /// <summary>
        /// input weights
        /// </summary>
        public double[,]? InputWeights { get; set; }
        /// <summary>
        /// middle weights
        /// </summary>
        public double[][,]? MiddleWeights { get; set; }
        /// <summary>
        /// output weights
        /// </summary>
        public double[,]? OutputWeights { get; set; }
        /// <summary>
        /// layers
        /// </summary>
        public int Layers { get; set; }
        /// <summary>
        /// mbias
        /// </summary>
        public double[,]? Mbias { get; set; }
        /// <summary>
        /// obias
        /// </summary>
        public double[]? Obias { get; set; }
    }
}
