/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

using System.Globalization;
using RogerHubEngine.RogerCore.Utils;
using RogerHubEngine.Engine.UI;
using RogerHubEngine.Engine.RogerCore;

using static RogerHubEngine.Engine.UI.CUI;

namespace RogerHubEngine.Client.Interfaces
{
    /// <summary>
    /// NeuralNetwork manual interface
    /// </summary>
    public class NeuralNetworkInterface()
    {
        /// <summary>
        /// Calling up the NeuralNetwork manual interface
        /// </summary>
        public static void StartInterface(NeuralNetwork? _neuralNetwork, Parameters _params)
        {
            Console.CursorVisible = true;

            while (true)
            {
                Console.Clear();
                Send("Enter \"save\" to fix the state of the neural network in the file, for load at this point later. Or \"exit\" to exit to the main menu", MessageType.note);
                Console.WriteLine($"Roger have {_neuralNetwork?.inputNeurons?.Length} input neurons, and {_neuralNetwork?.outputNeurons?.Length} output neurons." +
                    $"Write input format: <datain1>,<datain2>,<datain3>...");
                DrawLine(ConsoleColor.DarkGreen, "Welcome to Yocto Roger v2.2! Manual interface", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                Console.Write("\nInput>>>");
                string? userInputString = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInputString))
                {
                    if (userInputString == "exit")
                        MainMenuInterface.StartInterface();
                    else if (userInputString == "save")
                    {
                        Console.Write("Please, enter the name of the file without extenshion: ");

                        try
                        {
                            string input = Console.ReadLine() ?? string.Empty;

                            if (input is string fileName && !string.IsNullOrEmpty(fileName))
                                IO.SaveNeuralNetworkStateToBin(IO.FixTheStateOfNeuralNetwork(_neuralNetwork, _params), fileName);

                            else if (input == string.Empty)
                                IO.SaveNeuralNetworkStateToBin(IO.FixTheStateOfNeuralNetwork(_neuralNetwork, _params));
                            else
                                Send("Incorrect input (-_0)", MessageType.error);
                        }
                        catch (Exception e)
                        {
                            InternalError($"{e}");
                        }
                    }
                    else if (RogerMath.CleanInput(userInputString, out string cleanedUserInputString))
                    {
                        string[] userInputChecked = cleanedUserInputString.Split(',');
                        if (userInputChecked.Length == _neuralNetwork?.inputNeurons?.Length)
                        {
                            int[] userInput = new int[_neuralNetwork.inputNeurons.Length];
                            for (int i = 0; i < userInput.Length; i++)
                                userInput[i] = Convert.ToInt32(userInputChecked[i], CultureInfo.InvariantCulture);

                            NeuralNetwork.ForwardPropagation(_neuralNetwork, userInput);

                            Console.Write("Output>>>");
                            for (int i = 0; i < _neuralNetwork?.outputNeurons?.Length; i++)
                                Console.Write(_neuralNetwork.outputNeurons?[i] + " ");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        else
                            Send("It looks like you entered the wrong amount of information for the neurons or made a mistake with the command. No worries — it happens.", MessageType.error);
                    }
                }
                else
                    Send("Incorrect input (-_0)", MessageType.error);
            }
        }
    }
}
