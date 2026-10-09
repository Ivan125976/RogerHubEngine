/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

using RogerHubEngine.Client;
using RogerHubEngine.Engine.UI;
using System.Diagnostics.CodeAnalysis;
using static RogerHubEngine.Engine.UI.CUI;
using static RogerHubEngine.RogerCore.UtilityTools.RogerMath;

namespace RogerHubEngine.Engine.RogerCore
{
    /// <summary>
    /// Main Neural Network class
    /// </summary>

    public class NeuralNetwork(Parameters param) : IDisposable
    {
        private readonly Parameters _param = param;
        /// <summary>
        /// Flag indicating whether Roger has been created
        /// </summary>
        public bool rogerIsCreated = false;

        /// <summary>
        /// An array containing training data
        /// </summary>
        public double[,]? educationArray;

        /// <summary>
        /// Array of input neurons
        /// </summary>
        public int[]? inputNeurons;

        /// <summary>
        /// Array of middle neurons
        /// </summary>
        public double[,]? middleNeurons;

        /// <summary>
        /// Array of output neurons
        /// </summary>
        public double[]? outputNeurons;

        /// <summary>
        /// Array of input weights (I->M)
        /// </summary>
        public double[,]? inputWeights;

        /// <summary>
        /// Array of middle weights (M->M)
        /// </summary>
        public double[][,]? middleWeights;

        /// <summary>
        /// Array of output weights (M->O)
        /// </summary>
        public double[,]? outputWeights;

        /// <summary>
        /// Array of middle biases
        /// </summary>
        public double[,]? Mbias;

        /// <summary>
        /// Array of output biases
        /// </summary>
        public double[]? Obias;

        /// <summary>
        /// Generates a DropOut subsystem table
        /// </summary>
        /// <returns></returns>
        public float[,] GenerateDropOut()
        {
#if DEBUG
            Console.WriteLine("DropOut Matrix = ");
#endif
            float[,] masks = new float[_param.layers - 2, _param.middleNeuronsCount];
            float keepProb = 1.00f - _param.DropOutPercent * 0.01f;

            if (_param.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        masks[i, j] = 1.0f;
#if DEBUG
                        Console.Write(masks[i, j] + " ");
#endif
                    }
#if DEBUG
                    Console.WriteLine();
#endif
                }
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        if (rand.NextDouble() < _param.DropOutPercent / 100.0)
                            masks[i, j] = 0;
                        else
                            try
                            {
                                masks[i, j] = 1.0f / keepProb;
                            }
                            catch
                            {
                                InternalError("Division by zero. The dropout rate cannot be 100.");
                            }
#if DEBUG
                        Console.Write(masks[i, j] + " ");
#endif
                    }
#if DEBUG
                    Console.WriteLine();
#endif
                }
            }
            return masks;
        }

        /// <summary>
        /// Sum weights (I->M)
        /// </summary>
        /// <param name="oldweights">Input weights</param>
        /// <param name="oldNeurons">Input neurons</param>
        /// <param name="newNeurons">Middle neurons</param>
        /// <param name="biases">Middle biases</param>
        public static void SumWeights(double[,] oldweights, int[] oldNeurons, double[,] newNeurons, double[,] biases)
        {

#if DEBUG
            Console.Write("Sum of weights ([]->[,]) - ");
#endif
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[0, i];
                newNeurons[0, i] = Tanh(temp);
#if DEBUG
                Console.Write(newNeurons[0, i] + " ");
#endif
            }
#if DEBUG
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// Sum weights (M->M)
        /// </summary>
        /// <param name="oldweights">Middle weights</param>
        /// <param name="neurons">Middle neurons</param>
        /// <param name="biases">Middle biases</param>
        /// <param name="layer">Layer</param>
        public static void SumWeights(double[,] oldweights, double[,] neurons, double[,] biases, int layer) //нахождение новых нейронов (middle -> middle)
        {

#if DEBUG
            Console.Write("Sum of weights ([,]->[,]) - ");
#endif
            for (int i = 0; i < neurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < neurons.GetLength(1); j++)
                    temp += oldweights[j, i] * neurons[layer, j];
                temp += biases[layer + 1, i];
                neurons[layer + 1, i] = Tanh(temp);
#if DEBUG
                Console.Write(neurons[layer + 1, i] + " ");
#endif
            }
#if DEBUG
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// Sum weights (M->O)
        /// </summary>
        /// <param name="oldweights">Middle weights</param>
        /// <param name="oldNeurons">Middle neurons</param>
        /// <param name="newNeurons">Output neurons</param>
        /// <param name="biases">Output biases</param>
        public static void SumWeights(double[,] oldweights, double[,] oldNeurons, double[] newNeurons, double[] biases) //нахождение новых нейронов (middle -> output)
        {
#if DEBUG
            Console.Write("Sum of weights ([,]->[]) - ");
#endif
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[oldNeurons.GetLength(0) - 1, j];
                temp += biases[i];
                newNeurons[i] = Tanh(temp);
#if DEBUG
                Console.Write(newNeurons[i] + " ");
#endif
            }
#if DEBUG
            Console.WriteLine();
#endif
        }

        /// <summary>
        /// Write array to input neurons
        /// </summary>
        /// <param name="data">Data.</param>
        public void WriteToNN(int[] data)
        {
            if (inputNeurons != null && inputNeurons.Length == data.Length)
                for (int i = 0; i < inputNeurons.Length; i++)
                    inputNeurons[i] = data[i];
            else
                Send("NeuralNetwork.WriteToNN>The size of the neuron array and the data array do not match, it is impossible to write data", MessageType.error);
        }

        /// <summary>
        /// Forward propogation algorithm
        /// </summary>
        /// <param name="NN">Neural network</param>
        /// <param name="input">User input</param>
        public static void ForwardPropagation(NeuralNetwork NN, int[] input)
        {
            if (!NN.Initialized()) return;

            NN.WriteToNN(input);

            SumWeights(NN.inputWeights, NN.inputNeurons, NN.middleNeurons, NN.Mbias);

            for (int l = 0; l < NN.middleWeights.GetLength(1) - 1; l++)
                SumWeights(NN.middleWeights[l], NN.middleNeurons, NN.Mbias, l);

            SumWeights(NN.outputWeights, NN.middleNeurons, NN.outputNeurons, NN.Obias);
        }

        /// <summary>
        /// Forward propagation algorithm with dropout
        /// </summary>
        /// <param name="NN">Neural network</param>
        /// <param name="input">User input</param>
        /// <param name="dropOutMatrix">DropOut Matrix</param>
        public static void ForwardPropagation(NeuralNetwork NN, int[] input, float[,] dropOutMatrix)
        {
            if (!NN.Initialized()) return;

            NN.WriteToNN(input);

            SumWeights(NN.inputWeights, NN.inputNeurons, NN.middleNeurons, NN.Mbias);
            for (int i = 0; i < NN.middleNeurons.GetLength(1); i++)
                NN.middleNeurons[0, i] *= dropOutMatrix[0, i];

            for (int l = 0; l < NN.middleWeights.GetLength(1) - 1; l++)
            {
                SumWeights(NN.middleWeights[l], NN.middleNeurons, NN.Mbias, l);
                for (int j = 0; j < NN.middleNeurons.GetLength(1); j++)
                    NN.middleNeurons[l + 1, j] *= dropOutMatrix[l + 1, j];
            }

            SumWeights(NN.outputWeights, NN.middleNeurons, NN.outputNeurons, NN.Obias);
        }

        /// <summary>
        /// Checks whether the neural network's core variables have been created.
        /// </summary>
        /// <returns>If neural network isn't null, returns true</returns>
        [MemberNotNullWhen(true, nameof(Mbias), nameof(Obias),
                              nameof(inputWeights), nameof(outputWeights),
                              nameof(middleWeights), nameof(inputNeurons),
                              nameof(outputNeurons), nameof(middleNeurons))]
        public bool Initialized()
        {
            return Mbias != null
                && Obias != null
                && inputWeights != null
                && outputWeights != null
                && middleWeights != null
                && inputNeurons != null
                && outputNeurons != null
                && middleNeurons != null;
        }

        #region DisposeSupport

        /// <summary>
        /// Neural network disposed?
        /// </summary>
        public bool isDisposed = false;

        /// <summary>
        /// Dispose Neural Network
        /// </summary>
        public void Dispose()
        {
            if (isDisposed) return;

            isDisposed = true;

            Mbias = null;
            Obias = null;
            inputWeights = null;
            outputWeights = null;
            middleWeights = null;
            inputNeurons = null;
            outputNeurons = null;
            middleNeurons = null;

            GC.SuppressFinalize(this);
        }
    }
    #endregion
}
