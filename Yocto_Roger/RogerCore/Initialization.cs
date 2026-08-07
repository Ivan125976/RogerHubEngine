using Yocto_Roger.RogerCore.UtilityTools;
using Yocto_Roger.UI.CUI;
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger.RogerCore
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
    /// Contains all types of initialization
    /// </summary>
    public enum InitType
    {
        /// <summary>
        /// Xavier Uniform Initialization
        /// </summary>
        xavier_uniform,

        /// <summary>
        /// Xavier Normal Initialization
        /// </summary>
        xavier_normal
    }

    /// <summary>
    /// Class for initializing arrays of weights
    /// </summary>
    public class Initialization()
    {
        /// <summary>
        /// Creates an array of middle weights.
        /// </summary>
        /// <param name="weights">Array of middle weights</param>
        /// <param name="size">Size of array</param>
        public static void CreateMiddleWeights(double[][,] weights, int size)
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = new double[size, size];
        }

        /// <summary>
        /// Xavier Uniform method for two-dimensional weight arrays
        /// </summary>
        /// <param name="weights">Array of weights</param>
        /// <param name="type">Type of initialization</param>

        public static void Init(double[,] weights, InitType type)
        {
#if DEBUG
            Console.Write($"weights[,] = \n");
#endif
            switch (type)
            {
                case InitType.xavier_uniform:
                    double limit = (double)Math.Sqrt(6.0 / (weights.GetLength(0) + weights.GetLength(1)));
#if DEBUG
                    Send($"Xavier Uniform Initialization; limit = {limit}", MessageType.note);
#endif
                    for (int i = 0; i < weights.GetLength(0); i++)
                    {
                        for (int j = 0; j < weights.GetLength(1); j++)
                        {
                            weights[i, j] = RogerMath.rand.NextDouble() * limit * 2 - limit;
#if DEBUG
                            Console.Write($"{weights[i, j]} ");
#endif
                        }
#if DEBUG
                        Console.WriteLine();
#endif
                    }
#if DEBUG
                    Send("The weights have been successfully adjusted!");
#endif
                    break;

                case InitType.xavier_normal:
                    double stdDev = (double)Math.Sqrt(2.0 / (weights.GetLength(0) + weights.GetLength(1)));
#if DEBUG
                    Send($"Xavier Normal Initialization; Standard Deviation = {stdDev}", MessageType.note);
#endif
                    for (int i = 0; i < weights.GetLength(0); i++)
                    {
                        for (int j = 0; j < weights.GetLength(1); j++)
                        {
                            weights[i, j] = RogerMath.NextGaussian(0, stdDev);
#if DEBUG
                            Console.Write($"{weights[i, j]} ");
#endif
                        }
#if DEBUG
                        Console.WriteLine();
#endif
                    }
#if DEBUG
                    Send("The weights have been successfully adjusted!");
#endif
                    break;
            }

        }

        /// <summary>
        /// Xavier Uniform method an array of two-dimensional weight arrays (suitable for middle layers)
        /// </summary>
        /// <param name="weights">Array of weights</param>
        /// <param name="type">Type of initialization</param>
        public static void Init(double[][,] weights, InitType type)
        {
            if (weights.Length > 0)
            {
#if DEBUG
                Console.Write($"weights[][,] = \n");
#endif
                for (int i = 0; i < weights.Length; i++)
                {
                    switch (type)
                    {
                        case InitType.xavier_uniform:
                            double limit = (double)Math.Sqrt(6.0 / (weights[i].GetLength(0) + weights[i].GetLength(1)));
#if DEBUG
                            Send($"Xavier Uniform Initialization; limit = {limit}", MessageType.note);
#endif
                            for (int j = 0; j < weights[i].GetLength(0); j++)
                            {
                                for (int k = 0; k < weights[i].GetLength(1); k++)
                                {
                                    weights[i][j, k] = RogerMath.rand.NextDouble() * limit * 2 - limit;
#if DEBUG
                                    Console.Write($"{weights[i][j, k]} ");
#endif
                                }
#if DEBUG
                                Console.WriteLine();
#endif
                            }
#if DEBUG
                            Send(new string('=', Console.WindowWidth), MessageType.note);
#endif
                            break;

                        case InitType.xavier_normal:
                            double stdDev = (double)Math.Sqrt(2.0 / (weights[i].GetLength(0) + weights[i].GetLength(1)));
#if DEBUG
                            Send($"Xavier Normal Initialization; Standard Deviation = {stdDev}", MessageType.note);
#endif
                            for (int j = 0; j < weights[i].GetLength(0); j++)
                            {
                                for (int k = 0; k < weights[i].GetLength(1); k++)
                                {
                                    weights[i][j, k] = RogerMath.NextGaussian(0, stdDev);
#if DEBUG
                                    Console.Write($"{weights[i][j, k]} ");
#endif
                                }
#if DEBUG
                                Console.WriteLine();
#endif
                            }
#if DEBUG
                            Send(new string('=', Console.WindowWidth), MessageType.note);
#endif
                            break;
                    }
                }
#if DEBUG
                Send("The weights have been successfully adjusted!");
#endif
            }
        }
    }
}
