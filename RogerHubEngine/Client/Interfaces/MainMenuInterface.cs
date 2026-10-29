/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

using RogerHubEngine.Client.Interfaces.RRNNs;
using RogerHubEngine.Desktop.Resources;
using RogerHubEngine.Engine.UI;

using static RogerHubEngine.Client.EngineVersion;
using static RogerHubEngine.Engine.UI.CUI;

namespace RogerHubEngine.Client.Interfaces
{

    /// <summary>
    /// MainMenu interface
    /// </summary>
    public static class MainMenuInterface
    {
        /// <summary>
        /// Calling up the main menu
        /// </summary>
        public static void StartInterface()
        {
            Parameters parameters = new();

            while (true)
            {
                ConsoleManager.Clear();

                DrawLine(ConsoleColor.DarkMagenta, $"Welcome to the RogerHubEngine!", DateTime.Now.Date.ToString("dd/MM/yyyy"));

                Send("This project is still in the development stage.", MessageType.warning);

                if (specialName != Roadmap.R)
                    Send("This build may contain errors, as it is not a release!");

#if DEBUG
                Send("This is a DEBUG build. Good luck with the code, developer ;)");
#endif

                StringList mainMenu = new()
                {
                    borderStyle = StringList.BorderStyles.None
                };

                mainMenu.Add(" 1. Start Roger in training mode");
                mainMenu.Add(" 2. Load your roger (neural network) from the file");
                mainMenu.Add(" 3. Options for training mode...");
                mainMenu.Add(" 4. RRNNs Compilator...");
                mainMenu.Add(" 5. About...");
                mainMenu.Add(" 6. Exit from RogerHub");
                mainMenu.Draw();
                Console.Write(" > ");

                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("Starting Roger...");
                            CreateNeuralNetworkInterface.StartAI(0, parameters);
                            break;

                        case 2:
                            CreateNeuralNetworkInterface.StartAI(1, parameters);
                            break;

                        case 3:
                            SettingsInterface.StartInterface(parameters);
                            break;

                        case 4:
                            CreateNewRRNNSProject.StartInterface();
                            break;

                        case 5:
                            Console.Write(Resource.GetTextResource("RogerLogo1.txt") + '\n');
                            Console.WriteLine($" Github: https://github.com/Ivan125976/RogerHubEngine\n\n Authors: \n" +
                                $" Axolotl512 - AI, RRNNs and Client \n" +
                                $" d3ath-script - IO and compiling " +
                                $"\n\n RogerHubEngine v{engineVersion.Major}.{engineVersion.Minor}.{engineVersion.Build}\n" +
                                    $" RRNNs v{RRNNSCompilatorVersion.Major}.{RRNNSCompilatorVersion.Minor}.{RRNNSCompilatorVersion.Build} \n" +
                                    $" OpenRB does not support your version of the engine ");
                            ConsoleManager.PressKeyWait();
                            break;

                        case 6:
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("What?");
                            break;
                    }
                }
                else
                    Send("Incorrect input >:(", MessageType.error);
            }
        }
    }
}
