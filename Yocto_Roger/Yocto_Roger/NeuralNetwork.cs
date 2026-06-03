using System.Globalization;
using static Yocto_Roger.UI.UI;
using static Yocto_Roger.IO.MainIO;

namespace Yocto_Roger.Yocto_Roger
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    Yocto Roger 
*/
    internal class NeuralNetwork
    {
        public bool rogerIsCreated = false;

        public double[,]? educationArray;

        public int[]? inputNeurons;
        public double[,]? middleNeurons;
        public double[]? outputNeurons;

        public double[,]? inputWeights;
        public double[][,]? middleWeights;
        public double[,]? outputWeights;

        public double[,]? Mbias;
        public double[]?Obias;

        public void StartAI(int mode)
        {
            UI.UI user = new();
            Parameters param = new();
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    if (!File.Exists(param.knowledgeFile))
                    {
                        user.Send("I can't find the training file!", "error");
                        break;
                    }
                    Console.Write("SetUp education array and reading knowledge...");
                    string[] allLines = File.ReadAllLines(param.knowledgeFile);

                    string[] parsedString = allLines[0].Split(' ');
                    int[] input = AIMath.StringParse(parsedString[0], ',');
                    string[] splitingSecond = parsedString[1].Split(';');
                    double[] output = new double[splitingSecond.Length];
                    for (int j = 0; j < splitingSecond.Length; j++)
                        output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                    int length = input.Length + output.Length;

                    if (input.Length != param.inputNeuronsCount)
                    {
                        Console.WriteLine();
                        user.Send("NeuralNetwork.StartAI.InputNeurons>The training file doesn't match your neural network! (need value " + input.Length + " for Count of Input neurons)", "error");
                        Console.WriteLine("Do you want to change this parameter <Parameters.inputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                param.inputNeuronsCount = input.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }
                    else if (output.Length != param.outputNeuronsCount)
                    {
                        Console.WriteLine();
                        user.Send("NeuralNetwork.StartAI.OutputNeurons>The training file doesn't match your neural network! (need value " + output.Length + " for Count of Output neurons)", "error");
                        Console.WriteLine("Do you want to change this parameter <Parameters.outputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                param.outputNeuronsCount = output.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }

                    Console.CursorVisible = false;
                    user.Send("Everything is ready to create Roger!");

                    educationArray = new double[allLines.Length, length];

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        parsedString = allLines[i].Split(' ');
                        input = AIMath.StringParse(parsedString[0], ',');
                        splitingSecond = parsedString[1].Split(';');
                        for (int j = 0; j < input.Length; j++)
                            educationArray[i, j] = input[j];
                        for (int j = 0; j < splitingSecond.Length; j++)
                            output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                        for (int j = 0; j < splitingSecond.Length; j++)
                            educationArray[i, j + input.Length] = output[j];
                    }

                    for (int i = 0; i < educationArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < educationArray.GetLength(1); j++)
                            Console.Write(educationArray[i, j] + " ");
                        Console.WriteLine();
                    }

                    user.Send("done");
                    Console.Write("Initialization RogerHubEngine...");
                    inputNeurons = new int[param.inputNeuronsCount];
                    middleNeurons = new double[param.layers - 2, param.middleNeuronsCount];
                    outputNeurons = new double[param.outputNeuronsCount];
                    inputWeights = new double[param.inputNeuronsCount, param.middleNeuronsCount];
                    middleWeights = new double[param.layers - 3][,];
                    outputWeights = new double[param.middleNeuronsCount, param.outputNeuronsCount];
                    Mbias = new double[param.layers - 2, param.middleNeuronsCount];
                    Obias = new double[param.outputNeuronsCount];
                    user.Send("done");
                    Console.Write("Initialization biases...");
                    try
                    {
                        Biases.Init(ref Mbias);
                        Biases.Init(ref Obias);
                    }
                    catch (Exception ex)
                    {
                        user.Send("Failed to initialize the Biases: \n" + ex.Message, "error");
                        Thread.Sleep(5000);
                        break;
                    }
                    user.Send("done");
                    Console.Write("Initialization weights...");
                    try
                    {
                        Weights.Init(ref inputWeights);
                        Weights.Init(ref outputWeights);
                        Weights.Init(ref middleWeights);
                    }
                    catch (Exception ex)
                    {
                        user.Send($"Failed to initialize the Weights: \n{ex.Message}", "error");
                        Thread.Sleep(5000);
                        break;
                    }
                    user.Send("done");
                    user.Send("Initialization complete", "message");
                    Console.Write("Education...");
                    user.DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.WriteLine();
                    UI.Progressbar educationStatus = new(ConsoleColor.DarkGreen, 20, Console.CursorLeft, Console.CursorTop);

                    try
                    {
                        Training.Education(ref inputNeurons, ref middleNeurons, ref outputNeurons, ref inputWeights, ref middleWeights, ref outputWeights, ref Mbias, ref Obias, educationArray, educationStatus);
                    }
                    catch (Exception ex)
                    {
                        user.Send($"Filed to educate the data: \n{ex.Message}", "error");
                        Thread.Sleep(5000);
                        break;
                    }

                    educationStatus.Draw(100);
                    user.Send("\nEducation Complete");

                    Console.Write("Finishing...");
                    rogerIsCreated = true;
                    user.Send("done");
                    Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
                    Thread.Sleep(3000);
                    break;

                case 1:
                    Console.Write("Write an absolute path to your .json file\nSTRING> ");
                    string? userInput = Console.ReadLine();
                    if (userInput is string inputChecked && !string.IsNullOrEmpty(userInput) && Path.Exists(userInput))
                    {
                        Console.WriteLine("Loading your Roger... please wait :D");
                        InitNeuralNetwork(LoadNeuralNetworkStateFromJson(inputChecked), false);
                        rogerIsCreated = true;
                    }
                    else
                    {
                        user.Send("Incorrect input (-_0)", "error");
                        user.Send("Maybe file which you entered, doesn't exists, please check it and retry");
                    }
                    break;
            }
            if (rogerIsCreated)
            {
                float[,]? disabledDropOut = null;

                Console.CursorVisible = true;
                while (true)
                {
                    Console.Clear();
                    user.Send("Enter \"save\"  to fix the state of neural network in the file, for load at this point later. Or \"exit\" to exit to main menu", "warning");
                    Console.WriteLine($"Roger have {param.inputNeuronsCount} input neurons, and {param.outputNeuronsCount} output neurons." +
                        $"Write input format: <datain1>,<datain2>,<datain3>...");
                    user.DrawLine(ConsoleColor.DarkGreen, "Welcome to Yocto Roger v2.2! Manual interface", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.Write("\nInput>>>");
                    string? userInputString = Console.ReadLine();
                    if (!string.IsNullOrEmpty(userInputString))
                    {

                            string[] userInputChecked = userInputString.Split(',');
                            if (userInputChecked.Length == param.inputNeuronsCount)
                            {
                                int[] userInput = new int[param.inputNeuronsCount];
                                for (int i = 0; i < userInput.Length; i++)
                                    userInput[i] = Convert.ToInt32(userInputChecked[i], CultureInfo.InvariantCulture);
                                ForwardPropagation(userInput, inputNeurons!, inputWeights!, middleNeurons!, middleWeights!, Mbias!, outputNeurons!, Obias!, outputWeights!, disabledDropOut);
                                Console.Write("Output>>>");
                                for (int i = 0; i < outputNeurons!.Length; i++)
                                    Console.Write(outputNeurons[i] + " ");
                                Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                                Console.Clear();
                            }


                        if (userInputString == "exit")
                            return;

                        if (userInputString == "save")
                        {
                            Console.Write("Please, enter the path to the directory, where we going to save the file (to this directory, simple press the enter): ");
                            string input = Console.ReadLine() ?? string.Empty;

                            try
                            {
                                if (input is string path && !string.IsNullOrEmpty(path))
                                    SaveNeuralNetworkStateToJson(FixTheStateOfNeuralNetwork(false), path);

                                else if (input == string.Empty)
                                    SaveNeuralNetworkStateToJson(FixTheStateOfNeuralNetwork(false), Directory.GetCurrentDirectory());
                                else
                                    user.Send("Incorrect input (-_0)", "error");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Somethin' wrong with me, here's my exception: " + e.Message);
                            }
                        }
                    }
                    else
                    {
                        user.Send("Incorrect input (-_0)", "error");
                    }
                }
            }
        }

        public static float[,] GenerateDropOut()
        {
            Parameters param = new();

            if (param.isDebug)
                Console.WriteLine("DropOut Matrix = ");
            float[,] masks = new float[param.layers - 2, param.middleNeuronsCount];
            float keepProb = 1.00f - param.DropOutPercent * 0.01f;

            if (param.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        masks[i, j] = 1.0f;
                        if (param.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (param.isDebug)
                        Console.WriteLine();
                }
                return masks;
            }
            else
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        if (AIMath.rand.NextDouble() < param.DropOutPercent / 100.0)
                            masks[i, j] = 0;
                        else
                            masks[i, j] = 1.0f / keepProb;
                        if (param.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (param.isDebug)
                        Console.WriteLine();
                }
            }
            return masks;
        }
        public static void SumWeights(double[,] oldweights, int[] oldNeurons, double[,] newNeurons, double[,] biases) //нахождение новых нейронов (input -> middle)
        {
            Parameters param = new();

            if (param.isDebug)
                Console.Write("Sum of weights ([]->[,]) - ");
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[0, i];
                newNeurons[0, i] = AIMath.Tanh(temp);
                if (param.isDebug)
                    Console.Write(newNeurons[0, i] + " ");
            }
            if (param.isDebug)
                Console.WriteLine();
        }
        public static void SumWeights(double[,] oldweights, double[,] oldNeurons, double[,] newNeurons, double[,] biases, int layer) //нахождение новых нейронов (middle -> middle)
        {
            Parameters param = new();

            if (param.isDebug)
                Console.Write("Sum of weights ([,]->[,]) - ");
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[layer, j];
                temp += biases[layer + 1, i];
                newNeurons[layer + 1, i] = AIMath.Tanh(temp);
                if (param.isDebug)
                    Console.Write(newNeurons[layer + 1, i] + " ");
            }
            if (param.isDebug)
                Console.WriteLine();
        }
        public static void SumWeights(double[,] oldweights, double[,] oldNeurons, double[] newNeurons, double[] biases) //нахождение новых нейронов (middle -> output)
        {
            Parameters param = new();

            if (param.isDebug)
                Console.Write("Sum of weights ([,]->[]) - ");
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[oldNeurons.GetLength(0) - 1, j];
                temp += biases[i];
                newNeurons[i] = AIMath.Tanh(temp);
                if (param.isDebug)
                    Console.Write(newNeurons[i] + " ");
            }
            if (param.isDebug)
                Console.WriteLine();
        }

        public static void WriteToNN(int[] neurons, int[] writeArray)
        {
            UI.UI user = new();
            if (neurons.Length == writeArray.Length)
                for (int i = 0; i < neurons.Length; i++)
                    neurons[i] = writeArray[i];
            else
                user.Send("NeuralNetwork.WriteToNN>The size of the neuron array and the data array do not match, it is impossible to write data", "error");
        }

        public static void ForwardPropagation(int[] NNinput, int[] inputNeurons, double[,] inputWeights, double[,] middleNeurons, double[][,] middleWeights, double[,] middleBiases,
            double[] outputNeurons, double[] outputBiases, double[,] outputWeights, float[,]? dropOutMasks)
        {
            Parameters param = new();

            WriteToNN(inputNeurons, NNinput);

            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);

            if (dropOutMasks != null)
                for (int i = 0; i < middleNeurons.GetLength(1); i++)
                    middleNeurons[0, i] *= dropOutMasks[0, i];

            for (int l = 0; l < param.layers - 3; l++)
            {
                SumWeights(middleWeights[l], middleNeurons, middleNeurons, middleBiases, l);

                if (dropOutMasks != null)
                    for (int i = 0; i < middleNeurons.GetLength(1); i++)
                        middleNeurons[l + 1, i] *= dropOutMasks[l + 1, i];
            }

            SumWeights(outputWeights, middleNeurons, outputNeurons, outputBiases);
        }
    }
}
