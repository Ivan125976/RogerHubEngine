using Yocto_Roger.IO;
using Yocto_Roger.Yocto_Roger;

namespace Yocto_Roger.UI
{
    public class SetUpInterface(Parameters param, UI user, MainIO io, IO.Auxiliary auxiliaryIO)
    {
        private Parameters _param = param;
        private UI _user = user;
        private IO.MainIO _io = io;
        private IO.Auxiliary _auxiliaryIO = auxiliaryIO;
        /// <summary>
        /// Calling up the menu for setting values ​​and saving the file
        /// </summary>
        public void SetUpMenu()
        {
            int i = 0;
            while (i == 0)
            {
                Console.Clear();
                Console.Write($"""
                                        RogerHubEngine Training Options
                                            
                                        0. Save your roger settings in the file 
                                        1. Load your roger setting from the file

                                        2. Count of input neurons...{param.inputNeuronsCount}
                                        3. Count of middle neurons (all middle layers)...{param.middleNeuronsCount}
                                        4. Count of output neurons...{param.outputNeuronsCount}
                                        5. Count of Layers...{param.layers}
                                        6. Knowledge file...{param.knowledgeFile}
                                        7. DropOut sys percent...{param.DropOutPercent}% (0% - disable DropOut)
                                        8. Learning Rate...{param.learningRate}
                                        9. Passes...{param.passes}
                                        10. Exit 
                                        >>> 
                                        """);
                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "0":
                        Console.Write("""                           
                            How do you want to save roger?

                            1. INI
                            2. Json (recommended)

                            >>>
                            """);
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked))
                        {
                            switch (userInputChecked)
                            {
                                case 1:
                                    _io.SaveRoger();
                                    break;

                                case 2:
                                    _io.SaveRogerToJson();
                                    break;

                                default:
                                    user.Send("What?", "error");
                                    break;
                            }

                            Console.WriteLine("Your roger saved in this directory, let's go, check it!\n If file was not created, write it in issues on our GitHub please ;)" +
                                "\n Press any key to continue");
                            Console.ReadKey();
                        }
                        else
                            user.Send("Unknown input", "error");

                        break;

                    case "1":
                        Console.Write("Write an absolute path to the .roger of .json file please: ");

                        if (Console.ReadLine() is string input && !string.IsNullOrEmpty(input) && Path.Exists(input))
                        {
                            param.roger2 = input;

                            _auxiliaryIO.InitRogersData(roger: _io.LoadRoger());
                        }
                        else
                            user.Send("Incorrect input (-_0)", "error");
                        user.Send("Maybe file which you typed, doesn't exists or you typed not string, please recheck this 2 factors");
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*INPUT NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of input neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked1))
                        {
                            if (userInputChecked1 > 0)
                                param.inputNeuronsCount = userInputChecked1;
                            else
                                user.Send("Value out of range.", "error");
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of middle neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked2))
                        {
                            if (userInputChecked2 > 0)
                                param.middleNeuronsCount = userInputChecked2;
                            else
                                user.Send("Value out of range.", "error");
                        }
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*OUTPUT NEURONS PARAMETER*");
                        Console.Write("INT32> Enter new count of output neurons (> 0)...");
                        if (int.TryParse(Console.ReadLine(), out int userInputChecked3))
                        {
                            if (userInputChecked3 > 0)
                                param.outputNeuronsCount = userInputChecked3;
                            else
                                user.Send("Value out of range.", "error");
                        }
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*LAYERS PARAMETER*");
                        Console.Write("INT32> Enter new count of layers (> 2)...");
                        if (int.TryParse(Console.ReadLine(), out int layersCount))
                        {
                            if (layersCount > 2)
                                param.layers = layersCount;
                            else
                                user.Send("Value out of range.", "error");
                        }
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        string? file = Console.ReadLine();
                        if (File.Exists(file))
                            param.knowledgeFile = file;
                        else
                            user.Send("I couldn't find such a file :(", "error");
                        break;

                    case "7":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("FLOAT> Enter new DropOut percent (0–70)... ");
                        if (int.TryParse(Console.ReadLine(), out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                param.DropOutPercent = newDrop;
                            else
                                user.Send("Value out of range.", "error");
                        }
                        else
                            user.Send("Invalid input.", "error");
                        break;

                    case "8":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("FLOAT> Enter new learning rate (0,0 – 1,0)... ");
                        if (float.TryParse(Console.ReadLine(), out float newLR))
                        {
                            if (newLR > 0 && newLR <= 1.0)
                                param.learningRate = newLR;
                            else
                                user.Send("Learning rate out of range.", "error");
                        }
                        else
                            user.Send("Invalid input.", "error");
                        break;

                    case "9":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT32> Enter count of passes (> 0)... ");
                        if (int.TryParse(Console.ReadLine(), out int newPasses))
                        {
                            if (newPasses > 0)
                                param.passes = newPasses;
                            else
                                user.Send("Passes must be greater than zero.", "error");
                        }
                        else
                            user.Send("Invalid input.", "error");
                        break;

                    case "10":
                        i++;
                        break;
                }
            }
        }
    }
}
