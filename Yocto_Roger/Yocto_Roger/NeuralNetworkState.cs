namespace Yocto_Roger.Yocto_Roger
{
    public class NeuralNetworkState
    {
        public string? EducationArray { get; set; }
        public string? InputNeurons { get; set; }
        public string? MiddleNeurons { get; set; }
        public string? OutputNeurons { get; set; }

        public int InputNeuronsCount { get; set; }
        public int MiddleNeuronsCount { get; set; }
        public int OutputNeuronsCount { get; set; }

        public string? InputWeights { get; set; }
        public string? MiddleWeights { get; set; }
        public string? OutputWeights { get; set; }

        public int Layers { get; set; }

        public string? Mbias { get; set; }
        public string? Obias { get; set; }
    }
}
