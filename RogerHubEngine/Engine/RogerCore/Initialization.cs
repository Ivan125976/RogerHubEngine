using RogerHubEngine.Client;
using RogerHubEngine.Client.Interfaces;
using RogerHubEngine.Engine.UI;
using RogerHubEngine.RogerCore;
using RogerHubEngine.RogerCore.UtilityTools;
using static RogerHubEngine.Engine.UI.CUI;

namespace RogerHubEngine.Engine.RogerCore
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

        public static void Init(double[,] weights)
        {
            double limit = (double)Math.Sqrt(6 / (weights.GetLength(0) * 1.0 + weights.GetLength(1) * 1.0));
#if DEBUG
            Console.Write($"weights[,] = \n");
            Send($"Xaiver Uniform Initialization; limit = {limit}", MessageType.note);
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
        }

        /// <summary>
        /// Xavier Uniform method an array of two-dimensional weight arrays (suitable for middle layers)
        /// </summary>
        /// <param name="weights">Array of weights</param>

        public static void Init(double[][,] weights)
        {
            if (weights.Length > 0)
            {
                for (int i = 0; i < weights.Length; i++)
                {
                    double limit = (double)Math.Sqrt(6 / (weights[i].GetLength(0) * 1.0 + weights[i].GetLength(1) * 1.0));
#if DEBUG
                    Console.Write($"weights[][,] = \n");
                    Send($"Xaiver Uniform Initialization; limit = {limit}", MessageType.note);
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
                    Console.WriteLine(new string('=', Console.WindowWidth));
#endif
                }
#if DEBUG
                Send("The weights have been successfully adjusted!");
#endif
            }
        }

        /// <summary>
        /// Initializing all the classes. 
        /// </summary>
        /// <returns> Object that has a starting main menu function</returns>
        public static MainMenuInterface InitClasses() 
            /*
             * Warning!!!
             * 
             * This is a temporary solution, this looks like shit, so i will rewrite initialization soon, please sorry for this
             */
        {
            SettingsInterface settingsInterface = new();
            MainMenuInterface mainMenuInterface = new(null!);
            NeuralNetworkInterface neuralNetworkInterface = new(mainMenuInterface, null!);
            NeuralNetworkState neuralNetworkState = new();
            Training training = new(null!);
            NeuralNetwork nN = new(neuralNetworkState, training, neuralNetworkInterface, mainMenuInterface);

            //io._nN = nN;
            training.roger = nN;
            mainMenuInterface._roger = nN; 
            neuralNetworkInterface._neuralNetwork = nN;

            return mainMenuInterface;
        }
    }
}
