/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

using RogerHubEngine.Engine.RogerCore;
using RogerHubEngine.Engine.UI;
using System.Globalization;
using static RogerHubEngine.Engine.UI.CUI;

namespace RogerHubEngine.Client.Interfaces
{
    internal class CreateNeuralNetworkInterface
    {
        public static void StartAI(int mode, Parameters param, NeuralNetwork NN)
        {
            Console.WriteLine("StartAI in mode " + mode);
            switch (mode)
            {
                case 0:
                    if (!File.Exists(param.knowledgeFile))
                    {
                        Send("I can't find the training file! Please enter the path to it, in the settings", MessageType.error);
                        return;
                    }

                    if (param.rms_enabled && param.learningRate > 0.005 && param.layers < 6)
                    {
                        Send("I'm afraid the learning rate is too high for RMS", MessageType.warning);
                        Console.WriteLine("Do you want to continue? (Y/N)");
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.KeyChar.ToString().Equals("n", StringComparison.CurrentCultureIgnoreCase))
                            return;
                    }

                    Console.Write("SetUp education array and reading knowledge...");

                    string[] parsedString, splitingSecond, allLines;
                    double[] output;
                    int[] input;
                    int length;

                    try
                    {
                        allLines = File.ReadAllLines(param.knowledgeFile);

                        parsedString = allLines[0].Split(' ');
                        input = IO.SplitString(parsedString[0], ';');
                        splitingSecond = parsedString[1].Split(';');
                        output = new double[splitingSecond.Length];
                        for (int j = 0; j < splitingSecond.Length; j++)
                            output[j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                        length = input.Length + output.Length;

                        param.inputNeuronsCount = input.Length;
                        param.outputNeuronsCount = output.Length;
                    }
                    catch (Exception)
                    {
                        Send("Your training file is corrupted or is not in our format.", MessageType.error);
                        return;
                    }

                    Console.CursorVisible = false;
                    Send("Everything is ready to create Roger!");

                    NN.educationArray = new double[allLines!.Length, length];

                    for (int i = 0; i < allLines.Length; i++)
                    {
                        parsedString = allLines[i].Split(' ');
                        input = IO.SplitString(parsedString[0], ';');
                        splitingSecond = parsedString[1].Split(';');
                        for (int j = 0; j < input.Length; j++)
                            NN.educationArray[i, j] = input[j];
                        for (int j = 0; j < splitingSecond.Length; j++)
                            output![j] = Convert.ToDouble(splitingSecond[j], CultureInfo.InvariantCulture);
                        for (int j = 0; j < splitingSecond.Length; j++)
                            NN.educationArray[i, j + input.Length] = output![j];
                    }

                    for (int i = 0; i < NN.educationArray.GetLength(0); i++)
                    {
                        for (int j = 0; j < NN.educationArray.GetLength(1); j++)
                            Console.Write(NN.educationArray[i, j] + " ");
                        Console.WriteLine();
                    }

                    Send("done");
                    Console.Write("Initializing memory for Roger...");
                    NN.inputNeurons = new int[param.inputNeuronsCount];
                    NN.middleNeurons = new double[param.layers - 2, param.middleNeuronsCount];
                    NN.outputNeurons = new double[param.outputNeuronsCount];
                    NN.inputWeights = new double[param.inputNeuronsCount, param.middleNeuronsCount];
                    NN.middleWeights = new double[param.layers - 3][,];
                    NN.outputWeights = new double[param.middleNeuronsCount, param.outputNeuronsCount];
                    NN.Mbias = new double[param.layers - 2, param.middleNeuronsCount];
                    NN.Obias = new double[param.outputNeuronsCount];
                    Send("done");
                    Console.Write("Initialization weights...");
                    Initialization.Init(NN.inputWeights!);
                    Initialization.CreateMiddleWeights(NN.middleWeights!, param.middleNeuronsCount);
                    Initialization.Init(NN.middleWeights!);
                    Initialization.Init(NN.outputWeights!);
                    Send("done");
                    Send("Initialization complete", MessageType.message);
                    Console.Write("Education...");
                    DrawLine(ConsoleColor.DarkRed, "Creating your Roger, please wait :D", DateTime.Now.Date.ToString("dd/MM/yyyy"));
                    Console.WriteLine();
                    Progressbar educationStatus = new(ConsoleColor.DarkGreen, 20, Console.CursorLeft, Console.CursorTop);

                    Training.Education(NN, param, educationStatus);

                    educationStatus.Draw(100);
                    Send("\nEducation Complete");

                    Console.Write("StartAI finish");
                    NN.rogerIsCreated = true;
                    break;

                case 1:
                    Console.Write("Enter an absolute path to your .roger2 file\nSTRING> ");
                    string? userInput = Console.ReadLine();
                    if (userInput is string inputChecked && !string.IsNullOrEmpty(userInput))
                    {
                        Console.WriteLine("Loading your Roger... please wait :D");
                        try
                        {
                            if (File.Exists(inputChecked))
                            {
                                IO.InitNeuralNetwork(IO.LoadNeuralNetworkStateFromBin(inputChecked), _neuralNetworkState, param);
                                NN.rogerIsCreated = true;
                            }
                            else if (File.Exists(inputChecked + ".roger2"))
                            {
                                IO.InitNeuralNetwork(IO.LoadNeuralNetworkStateFromBin(inputChecked + ".roger2"), _neuralNetworkState, param);
                                NN.rogerIsCreated = true;
                            }
                        }
                        catch (MemoryPackSerializationException e)
                        {
                            Send("Error loading your roger! X( : \n", MessageType.error);
                            Send(e.Message, MessageType.error);
                        }
                    }
                    else
                    {
                        Send("Incorrect input (-_0)", MessageType.error);
                        Send("Maybe file that you entered, doesn't exists, please check it and retry");
                    }
                    Console.Write("StartAI finish");
                    break;
            }

            Console.WriteLine("Hello! I'm Roger, the neuron network from Emotion!");
            Thread.Sleep(3000);
            if (NN.rogerIsCreated)
                _neuralNetworkInterface.StartInterface();
        }
    }
}
