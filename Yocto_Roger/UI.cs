using System.Text;

namespace Yocto_Roger
/* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp. License
*/

{
    public class UI
    {
        static void Main()
        {
            Console.WriteLine("Configuring console...");
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
            SendMessage(ConsoleColor.Magenta, "Emotion ;) 2026    Yocto Roger");
            Thread.Sleep(3000);
            if (Parameters.isDebug == false)
                SendMessage(ConsoleColor.DarkMagenta, $"Welcome to the RogerHub! v.{Parameters.version}{Parameters.revision}");
            else
                SendMessage(ConsoleColor.DarkMagenta, $"Welcome to the RogerHub! v.{Parameters.version}{Parameters.revision} DEBUG MODE");
            int i = 0;
            while (true)
            {
                SendWarning("!WARNING! This project is still in the development stage.");
                Console.Write(" 1. Start Roger in training mode \n 2. Start Roger from the .roger file \n 3. Options for training mode \n 4. RRNNs settings \n 5. Exit of RogerHub \n >>> ");
                if (int.TryParse(Console.ReadLine(), out int value))
                {
                    switch (value)
                    {
                        case 1:
                            Console.WriteLine("Starting Roger...");
                            NeuralNetwork.StartAI(0);
                            break;

                        case 2:
                            Console.Clear();
                            Console.Write("1. Place the .roger2 file in the folder with Yocto Roger 2.1.exe\n2. Enter the file name .roger2 (with extension)\n>>> ");
                            Parameters.roger2 = Console.ReadLine();
                            if (File.Exists(Parameters.roger2))
                                NeuralNetwork.StartAI(1);
                            else
                                SendError("Roger2 doesn't found!");
                            break;

                        case 3:
                            SetUp();
                            break;

                        case 4:
                            SendMessage(ConsoleColor.DarkGreen, "You're in the  RRNN's settings ;)");
                            RRNNs.SetUpPins();
                            break;

                        case 5:
                            Environment.Exit(0);
                            break;

                        default:
                            switch (i++)
                            {
                                case 0:
                                    Console.WriteLine("What?");
                                    break;
                                case 1:
                                    Console.WriteLine("I dont understand... (0-0)");
                                    break;
                                case 2:
                                    Console.WriteLine("Again?");
                                    break;
                                case 3:
                                    Console.WriteLine("PLEASE STOP!!!");
                                    break;
                                case 4:
                                    Console.WriteLine("I'm going disconnect...");
                                    Thread.Sleep(300);
                                    Console.WriteLine("Bye ;(");
                                    Thread.Sleep(1000);
                                    Environment.Exit(0);
                                    break;
                            }
                            break;
                    }
                }
                else
                    SendError("Incorrect input >:(");
            }
        }

        public static int CountLines(string path)
        {
            int count = 0;
            using var reader = new StreamReader(path);
            while (reader.ReadLine() != null)
                count++;
            return count;
        }

        public static void SendMessage(ConsoleColor color, string message)
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = color;

            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(message);
            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void SetUp()
        {
            Console.Clear();

            string userInput = "";
            int i = 0;
            while (i == 0)
            {
                Console.Write($"RogerHub Training Options \n 1. Number of middle neurons...{Parameters.middleNeuronsCount} \n 2. Knowledge file...{Parameters.knowledgeFile} \n 3. DropOut sys percent...{Parameters.DropOutPercent}% \n 4. Learning Rate...{Parameters.learningRate} \n 5. Passes...{Parameters.passes} \n 6. Exit \n >>>");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("*MIDDLE NEURONS PARAMETER*");
                        Console.Write("INT16> Enter new middle neurons number (> 0)...");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int userInputChecked))
                        {
                            if (userInputChecked > 0)
                                Parameters.middleNeuronsCount = userInputChecked;
                            else
                                SendError("Value out of range.");
                        }
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("*KNOWLEDGE PARAMETER*");
                        Console.Write("STRING> Enter new knowledge file...");
                        userInput = Console.ReadLine();
                        if (File.Exists(userInput))
                        {
                            Parameters.knowledgeFile = userInput;
                        }
                        else
                            SendError("Knowledge file doesn't exists");
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("*DROPOUT PERCENT PARAMETER*");
                        Console.Write("INT16> Enter new DropOut percent (0–70)... ");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newDrop))
                        {
                            if (newDrop >= 0 && newDrop <= 70)
                                Parameters.DropOutPercent = newDrop;
                            else
                                SendError("Value out of range.");
                        }
                        else
                            SendError("Invalid input.");
                        break;

                    case "4":
                        Console.Clear();
                        Console.WriteLine("*LEARNING RATE PARAMETER*");
                        Console.Write("INT32> Enter new learning rate (0.001 – 1.0)... ");
                        userInput = Console.ReadLine();
                        if (float.TryParse(userInput, out float newLR))
                        {
                            if (newLR > 0 && newLR <= 1.0)
                                Parameters.learningRate = newLR;
                            else
                                SendError("Learning rate out of range.");
                        }
                        else
                            SendError("Invalid input.");
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("*PASSES PARAMETER*");
                        Console.Write("INT16> Enter passes count (> 0)... ");
                        userInput = Console.ReadLine();
                        if (int.TryParse(userInput, out int newPasses))
                        {
                            if (newPasses > 0)
                                Parameters.passes = newPasses;
                            else
                                SendError("Passes must be greater than zero.");
                        }
                        else
                            SendError("Invalid input.");
                        break;

                    case "6":
                        i++;
                        break;
                }
            }
        }

        public static void SendWarning(string message = "Warning!")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void SendError(string message = "Error!")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
