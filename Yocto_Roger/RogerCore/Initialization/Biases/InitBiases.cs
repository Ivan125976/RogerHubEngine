using Yocto_Roger.RogerCore.UtilityTools;
using static Yocto_Roger.UI.GUI.GUI;

namespace Yocto_Roger.RogerCore.Initialization.Biases
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Internal Biases lib
*/

    /// <summary> 
    /// Class for initializing arrays of biases
    /// </summary>
    public class InitBiases(Parameters param)
    {
        private readonly Parameters _param = param;
        /// <summary>
        /// Random filling of the array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public static void Init(ref double[] biases)
        {
            for (int i = 0; i < biases.Length; i++)
                biases[i] = 0;
        }

        /// <summary>
        /// Random filling of a two-dimensional array of biases
        /// </summary>
        /// <param name="biases">Array of biases</param>

        public static void Init(ref double[,] biases)
        {
#if DEBUG
            Console.Write($"biases[,] = \n");
#endif
            for (int i = 0; i < biases.GetLength(0); i++)
            {
                for (int j = 0; j < biases.GetLength(1); j++)
                {
                    biases[i, j] = RogerMath.rand.NextDouble() * 0.2 - 0.1;
#if DEBUG
                    Console.Write($"{biases[i, j]} ");
#endif
                }
#if DEBUG
                Console.WriteLine();
#endif
            }
#if DEBUG
            Send("The biases have been successfully adjusted!");
#endif
        }
    }
}
