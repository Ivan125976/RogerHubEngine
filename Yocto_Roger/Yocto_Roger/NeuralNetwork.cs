using System.Globalization;
using Yocto_Roger.IO;
using Yocto_Roger.Yocto_Roger;

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
    public class NeuralNetwork(Parameters param, UI.UI user, NeuralNetworkState nNState, IO.MainIO io, Weights weights, Biases biases, Training training, AIMath aiMath)
    {
        private Parameters _param = param;
        private UI.UI _user = user;
        private NeuralNetworkState _nNState = nNState;
        private MainIO _io = io;
        private Weights _weights = weights;
        private Biases _biases = biases;
        private Training _training = training;
        private AIMath _aiMath = aiMath;

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
            //UI.UI user = new();
            //Parameters param = new();
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    if (!File.Exists(_param.knowledgeFile))
                    {
                        _user.Send("I can't find the training file!", "error");
                        break;
                    }
                    Console.Write("SetUp education array and reading knowledge...");
                    string[] allLines = File.ReadAllLines(_param.knowledgeFile);

                    string[] parsedString = allLines[0].Split(' ');
                    int[] input = _aiMath.StringParse(parsedString[0], ',');
                    string[] splitingSecond = parsedString[1].Split(';');
                    double[] output = new double[splitingSecond.Length];
                    for (int j = 0; j < splitingSecond.Length; j++)
                        output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                    int length = input.Length + output.Length;

                    if (input.Length != _param.inputNeuronsCount)
                    {
                        Console.WriteLine();
                        _user.Send("NeuralNetwork.StartAI.InputNeurons>The training file doesn't match your neural network! (need value " + input.Length + " for Count of Input neurons)", "error");
                        Console.WriteLine("Do you want to change this parameter <Parameters.inputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                _param.inputNeuronsCount = input.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }
                    else if (output.Length != _param.outputNeuronsCount)
                    {
                        Console.WriteLine();
                        _user.Send("NeuralNetwork.StartAI.OutputNeurons>The training file doesn't match your neural network! (need value " + output.Length + " for Count of Output neurons)", "error");
                        Console.WriteLine("Do you want to change this parameter <Parameters.outputNeuronsCount> and restart NeuralNetwork? (y/n)");
                        ConsoleKeyInfo answer = Console.ReadKey();
                        switch (answer.KeyChar)
                        {
                            case 'y':
                                _param.outputNeuronsCount = output.Length;
                                Console.Clear();
                                StartAI(0);
                                break;
                        }
                        break;
                    }

                    Console.CursorVisible = false;
                    _user.Send("Everything is ready to create Roger!");

                    educationArray = new double[allLines.Length, length];

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        parsedString = allLines[i].Split(' ');
                        input = _aiMath.StringParse(parsedString[0], ',');
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

                    _user.Send("done");
                    Console.Write("Initialization RogerHubEngine...");
                    inputNeurons = new int[_param.inputNeuronsCount];
                    middleNeurons = new double[_param.layers - 2, _param.middleNeuronsCount];
                    outputNeurons = new double[_param.outputNeuronsCount];
                    inputWeights = new double[_param.inputNeuronsCount, _param.middleNeuronsCount];
                    middleWeights = new double[_param.layers - 3][,];
                    outputWeights = new double[_param.middleNeuronsCount, _param.outputNeuronsCount];
                    Mbias = new double[_param.layers - 2, _param.middleNeuronsCount];
                    Obias = new double[_param.outputNeuronsCount];
                    _user.Send("done");
                    Console.Write("Initialization biases...");
                    try
                    {
                        _biases.Init(ref Mbias);
                        _biases.Init(ref Obias);
                    }
                    catch (Exception ex)
                    {
                        _user.Send("Failed to initialize the Biases: \n" + ex.Message, "error");
                        Thread.Sleep(5000);
                        break;
                    }
                    _user.Send("done");
                    Console.Write("Initialization weights...");
                    try
                    {
                        _weights.Init(ref inputWeights);
                        _weights.Init(ref outputWeights);
                        _weights.Init(ref middleWeights);
                    }
                    catch (Exception ex)
                    {
                        _user.Send($"Failed to initialize the Weights: \n{ex.Message}", "error");
                        Thread.Sleep(5000);
                        break;
                    }
                    _user.Send("done");
                    _user.Send("Initialization complete", "message");
                    Console.Write("Education...");
                    _user.DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.WriteLine();
                    UI.Progressbar educationStatus = new(ConsoleColor.DarkGreen, 20, Console.CursorLeft, Console.CursorTop);

                    try
                    {
                        _training.Education(ref inputNeurons, ref middleNeurons, ref outputNeurons, ref inputWeights, ref middleWeights, ref outputWeights, ref Mbias, ref Obias, educationArray, educationStatus);
                    }
                    catch (Exception ex)
                    {
                        _user.Send($"Failed to educate the data: \n{ex.Message}", "error");
                        Thread.Sleep(5000);
                        break;
                    }

                    educationStatus.Draw(100);
                    _user.Send("\nEducation Complete");

                    Console.Write("Finishing...");
                    rogerIsCreated = true;
                    _user.Send("done");
                    Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
                    Thread.Sleep(3000);
                    break;

                case 1:
                    Console.Write("Write an absolute path to your .json file\nSTRING> ");
                    string? userInput = Console.ReadLine();
                    if (userInput is string inputChecked && !string.IsNullOrEmpty(userInput) && Path.Exists(userInput))
                    {
                        Console.WriteLine("Loading your Roger... please wait :D");
                        _io.InitNeuralNetwork(_io.LoadNeuralNetworkStateFromJson(inputChecked), false);
                        rogerIsCreated = true;
                    }
                    else
                    {
                        _user.Send("Incorrect input (-_0)", "error");
                        _user.Send("Maybe file which you entered, doesn't exists, please check it and retry");
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
                    _user.Send("Enter \"save\"  to fix the state of neural network in the file, for load at this point later. Or \"exit\" to exit to main menu", "warning");
                    Console.WriteLine($"Roger have {_param.inputNeuronsCount} input neurons, and {_param.outputNeuronsCount} output neurons." +
                        $"Write input format: <datain1>,<datain2>,<datain3>...");
                    _user.DrawLine(ConsoleColor.DarkGreen, "Welcome to Yocto Roger v2.2! Manual interface", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.Write("\nInput>>>");
                    string? userInputString = Console.ReadLine();
                    if (!string.IsNullOrEmpty(userInputString))
                    {

                            string[] userInputChecked = userInputString.Split(',');
                            if (userInputChecked.Length == _param.inputNeuronsCount)
                            {
                                int[] userInput = new int[_param.inputNeuronsCount];
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
                                    _io.SaveNeuralNetworkStateToJson(_io.FixTheStateOfNeuralNetwork(false), path);

                                else if (input == string.Empty)
                                    _io.SaveNeuralNetworkStateToJson(_io.FixTheStateOfNeuralNetwork(false), Directory.GetCurrentDirectory());
                                else
                                    _user.Send("Incorrect input (-_0)", "error");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Somethin' wrong with me, here's my exception: " + e.Message);
                            }
                        }
                    }
                    else
                    {
                        _user.Send("Incorrect input (-_0)", "error");
                    }
                }
            }
        }

        public float[,] GenerateDropOut()
        {
            Parameters param = new();

            if (_param.isDebug)
                Console.WriteLine("DropOut Matrix = ");
            float[,] masks = new float[_param.layers - 2, _param.middleNeuronsCount];
            float keepProb = 1.00f - _param.DropOutPercent * 0.01f;

            if (_param.DropOutPercent == 0)
            {
                for (int i = 0; i < masks.GetLength(0); i++)
                {
                    for (int j = 0; j < masks.GetLength(1); j++)
                    {
                        masks[i, j] = 1.0f;
                        if (_param.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (_param.isDebug)
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
                        if (AIMath.rand.NextDouble() < _param.DropOutPercent / 100.0)
                            masks[i, j] = 0;
                        else
                            masks[i, j] = 1.0f / keepProb;
                        if (_param.isDebug)
                            Console.Write(masks[i, j] + " ");
                    }
                    if (_param.isDebug)
                        Console.WriteLine();
                }
            }
            return masks;
        }
        public void SumWeights(double[,] oldweights, int[] oldNeurons, double[,] newNeurons, double[,] biases) //нахождение новых нейронов (input -> middle)
        {

            if (_param.isDebug)
                Console.Write("Sum of weights ([]->[,]) - ");
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.Length; j++)
                    temp += oldweights[j, i] * oldNeurons[j];
                temp += biases[0, i];
                newNeurons[0, i] = _aiMath.Tanh(temp);
                if (_param.isDebug)
                    Console.Write(newNeurons[0, i] + " ");
            }
            if (_param.isDebug)
                Console.WriteLine();
        }
        public void SumWeights(double[,] oldweights, double[,] oldNeurons, double[,] newNeurons, double[,] biases, int layer) //нахождение новых нейронов (middle -> middle)
        {

            if (_param.isDebug)
                Console.Write("Sum of weights ([,]->[,]) - ");
            for (int i = 0; i < newNeurons.GetLength(1); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[layer, j];
                temp += biases[layer + 1, i];
                newNeurons[layer + 1, i] = _aiMath.Tanh(temp);
                if (_param.isDebug)
                    Console.Write(newNeurons[layer + 1, i] + " ");
            }
            if (_param.isDebug)
                Console.WriteLine();
        }
        public void SumWeights(double[,] oldweights, double[,] oldNeurons, double[] newNeurons, double[] biases) //нахождение новых нейронов (middle -> output)
        {
            if (_param.isDebug)
                Console.Write("Sum of weights ([,]->[]) - ");
            for (int i = 0; i < newNeurons.GetLength(0); i++)
            {
                double temp = 0;
                for (int j = 0; j < oldNeurons.GetLength(1); j++)
                    temp += oldweights[j, i] * oldNeurons[oldNeurons.GetLength(0) - 1, j];
                temp += biases[i];
                newNeurons[i] = _aiMath.Tanh(temp);
                if (_param.isDebug)
                    Console.Write(newNeurons[i] + " ");
            }
            if (_param.isDebug)
                Console.WriteLine();
        }

        public void WriteToNN(int[] neurons, int[] writeArray)
        {
            if (neurons.Length == writeArray.Length)
                for (int i = 0; i < neurons.Length; i++)
                    neurons[i] = writeArray[i];
            else
                _user.Send("NeuralNetwork.WriteToNN>The size of the neuron array and the data array do not match, it is impossible to write data", "error");
        }

        public void ForwardPropagation(int[] NNinput, int[] inputNeurons, double[,] inputWeights, double[,] middleNeurons, double[][,] middleWeights, double[,] middleBiases,
            double[] outputNeurons, double[] outputBiases, double[,] outputWeights, float[,]? dropOutMasks)
        {
            Parameters param = new();

            WriteToNN(inputNeurons, NNinput);

            SumWeights(inputWeights, inputNeurons, middleNeurons, middleBiases);

            if (dropOutMasks != null)
                for (int i = 0; i < middleNeurons.GetLength(1); i++)
                    middleNeurons[0, i] *= dropOutMasks[0, i];

            for (int l = 0; l < _param.layers - 3; l++)
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
