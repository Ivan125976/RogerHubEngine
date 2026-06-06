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
        /// Random filling of a two-dimensional array of weights
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public void Init(ref double[,] weights)
        {

            if (_param.isDebug)
                Console.Write($"weights[,] = \n");
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = RogerMath.rand.NextDouble() * 0.2 - 0.1;
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
        /// Random filling an array of two-dimensional weight arrays (suitable for middle layers)
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public void Init(ref double[][,] weights)
        {

            if (_param.isDebug)
                Console.Write($"weights[][,] = \n");
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = new double[_param.middleNeuronsCount, _param.middleNeuronsCount];
                for (int j = 0; j < weights[i].GetLength(0); j++)
                {
                    for (int k = 0; k < weights[i].GetLength(1); k++)
                    {
                        weights[i][j, k] = RogerMath.rand.NextDouble() * 0.2 - 0.1;
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
    }
}
