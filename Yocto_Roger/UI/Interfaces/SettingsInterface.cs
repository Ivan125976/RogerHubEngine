using Yocto_Roger.RogerCore;
using Yocto_Roger.UI.CUI;
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger.UI.Interfaces
{

    /// <summary>
    /// Settings interface
    /// </summary>
    public class SettingsInterface(Parameters param, IO io) : IUserInterface
    {
        private readonly IO _io = io;

        /// <summary>
        /// Calling up the menu for setting values ​​and saving the file
        /// </summary>
        public void StartInterface()
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear();
                Console.Write($"""
                                        RogerHubEngine Training Options
                                            
                                        0. Save your roger settings in the file 
                                        1. Load your roger setting from the file

                                        2. Count of middle neurons (all middle layers)...{param.middleNeuronsCount}
                                        3. Count of Layers...{param.layers}
                                        4. Knowledge file...{param.knowledgeFile}
                                        5. DropOut sys percent...{param.DropOutPercent}%
                                        6. Learning Rate...{param.learningRate}
                                        7. Passes...{param.passes}
                                        8. Type of initialization...{param.initType}
                                        9. RMS Enabled...{param.rms_enabled}
                                        10. RMS Decay...{param.rms_decay}
                                        11. Exit
                                        >>> 
                                        """);
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        Console.WriteLine("Enter the name of the new file...");
                        string? fileName = Console.ReadLine();

                        if (fileName != null)
                        {
                            _io.SaveRogerToJson(fileName);

                            Send($" Your file saved> {fileName}.params\n Press any key to continue");
                            Console.ReadKey(true);
                        }

                        break;

                    case "1":
                        Console.Write("Write a name of your .params file please...");

                        if (Console.ReadLine() is string input && !string.IsNullOrEmpty(input) && (File.Exists(input) || File.Exists(input + ".params")))
                        {

                            if (File.Exists(input))
                            {
                                param.roger2 = input;
                                _io.InitRogersData(roger: _io.LoadRoger());
                            }
                            else if (File.Exists(input + ".params"))
                            {
                                param.roger2 = input + ".params";
                                _io.InitRogersData(roger: _io.LoadRoger());
                            }
                        }
                        else
                            Send("Maybe file which you typed, doesn't exists or you typed not string, please recheck this 2 factors", MessageType.error);
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of middle neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked2))
                        {
                            if (userInputChecked2 > 0)
                                param.middleNeuronsCount = userInputChecked2;
                            else
                                Send("Value out of range.", MessageType.error);
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*LAYERS PARAMETER*");
                        Console.Write("INT32> Enter new count of layers (> 2)...");
                        if (int.TryParse(Console.ReadLine(), out int layersCount))
                        {
                            if (layersCount > 2)
                                param.layers = layersCount;
                            else
                                Send("Value out of range.", MessageType.error);
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        string? file = Console.ReadLine();
                        if (File.Exists(file))
                            param.knowledgeFile = file;
                        else if (File.Exists(file + ".know"))
                            param.knowledgeFile = file + ".know";
                        else if (File.Exists(file + ".txt"))
                            param.knowledgeFile = file + ".txt";
                        else
                            Send("I couldn't find such a file :(", MessageType.error);
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("FLOAT> Enter new DropOut percent (0–70)... ");
                        if (int.TryParse(Console.ReadLine(), out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                param.DropOutPercent = newDrop;
                            else
                                Send("Value out of range.", MessageType.error);
                        }
                        else
                            Send("Invalid input.", MessageType.error);
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("FLOAT> Enter new learning rate (0,0 – 1,0)... ");
                        if (float.TryParse(Console.ReadLine(), out float LR))
                        {
                            if (LR > 0 && LR <= 1.0)
                                param.learningRate = LR;
                            else
                                Send("Learning rate out of range.", MessageType.error);
                        }
                        else
                            Send("Invalid input.", MessageType.error);
                        break;

                    case "7":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT32> Enter count of passes (> 0)... ");
                        if (int.TryParse(Console.ReadLine(), out int newPasses))
                        {
                            if (newPasses > 0)
                                param.passes = newPasses;
                            else
                                Send("Passes must be greater than zero.", MessageType.error);
                        }
                        else
                            Send("Invalid input.", MessageType.error);
                        break;

                    case "8":
                        Console.Clear();
                        Console.WriteLine("*INITIALIZATION TYPE*");

                        Console.WriteLine("""
                            
                            1. Xavier Uniform
                            2. Xavier Normal

                            """);

                        Console.Write("INT32> Enter number of new initialization method...");
                        if (int.TryParse(Console.ReadLine(), out int newInitMethod))
                        {
                            switch(newInitMethod)
                            {
                                case 1:
                                    param.initType = InitType.xavier_uniform;
                                    break;

                                case 2:
                                    param.initType = InitType.xavier_normal;
                                    break;
                            }
                        }
                        else
                            Send("Invalid input.", MessageType.error);
                        break;

                    case "9":
                        Console.Clear();
                        Console.WriteLine("*RMS PROP OPTIMIZATION*");
                        Send("This optimization speeds up training, but consumes twice as much memory allocated to the neural network during training", MessageType.warning);
                        Send("It is not recommended to enable this for networks with fewer than 6 layers.", MessageType.warning);
                        Console.Write("BOOL> Enter the switch value (True/False)... ");
                        if (bool.TryParse(Console.ReadLine(), out bool RMS))
                            param.rms_enabled = RMS;
                        else
                            Send("Invalid input.", MessageType.error);
                        break;

                    case "10":
                        Console.Clear();
                        if (param.rms_enabled)
                        {
                            Console.WriteLine("*RMS PROP DECAY OPTIMIZATION*");
                            Console.Write("DOUBLE> Enter new RMS Decay (0,9 - 0,999)... ");
                            if (float.TryParse(Console.ReadLine(), out float RMSDECAY))
                            {
                                if (!(RMSDECAY < 0.9f || RMSDECAY > 0.999f))
                                    param.rms_decay = RMSDECAY;
                                else
                                    Send("Invalid input.", MessageType.error);
                            }
                            else
                                Send("Invalid input.", MessageType.error);
                            break;
                        }
                        else
                        {
                            Send("RMS not enabled", MessageType.error);
                            break;
                        }

                    case "11":
                        exit = true;
                        break;
                }
            }
        }
    }
}
