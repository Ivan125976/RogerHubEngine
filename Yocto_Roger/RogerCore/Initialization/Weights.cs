using Yocto_Roger.UI.GUI;
using Yocto_Roger.Yocto_Roger.UtilityTools;
using static Yocto_Roger.UI.GUI.GUI;

namespace Yocto_Roger.Yocto_Roger.Initialization
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Internal weights lib
*/

    /// <summary>
    /// Class for initializing arrays of weights
    /// </summary>

    public class Weights(Parameters param)
    {
        private readonly Parameters _param = param;
        /// <summary>
        /// Xavier Uniform method for two-dimensional weight arrays
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public void Init(ref double[,] weights)
        {
            double limit = (double)Math.Sqrt(6 / (weights.GetLength(0) * 1.0 + weights.GetLength(1) * 1.0));
            if (_param.isDebug)
            {
                Console.Write($"weights[,] = \n");
                Send($"Xaiver Uniform Initialization; limit = {limit}", MessageType.warning);
            }
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = RogerMath.rand.NextDouble() * limit * 2 - limit;
                    if (_param.isDebug)
                        Console.Write($"{weights[i, j]} ");
                }
                if (_param.isDebug)
                    Console.WriteLine();
            }
            if (_param.isDebug)
                Send("The weights have been successfully adjusted!");
        }

        /// <summary>
        /// Xavier Uniform method an array of two-dimensional weight arrays (suitable for middle layers)
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public void Init(ref double[][,] weights)
        {
            if (weights.Length > 0)
            {
                double limit = (double)Math.Sqrt(6 / (weights.GetLength(0) * 1.0 + weights.GetLength(1) * 1.0));
                if (_param.isDebug)
                {
                    Console.Write($"weights[][,] = \n");
                    Send($"Xaiver Uniform Initialization; limit = {limit}", MessageType.warning);
                }
                for (int i = 0; i < weights.Length; i++)
                {
                    weights[i] = new double[_param.middleNeuronsCount, _param.middleNeuronsCount];
                    for (int j = 0; j < weights[i].GetLength(0); j++)
                    {
                        for (int k = 0; k < weights[i].GetLength(1); k++)
                        {
                            weights[i][j, k] = RogerMath.rand.NextDouble() * limit * 2 - limit;
                            if (_param.isDebug)
                                Console.Write($"{weights[i][j, k]} ");
                        }
                        if (_param.isDebug)
                            Console.WriteLine();
                    }
                    if (_param.isDebug)
                        Console.WriteLine(new string('=', Console.WindowWidth));
                }
                if (_param.isDebug)
                    Send("The weights have been successfully adjusted!");
            }
            else if (_param.isDebug)
                Send("The neural network doesn't have average weights. To fix this, select a number of layers greater than 3   0_0", MessageType.warning);
        }
    }
}
