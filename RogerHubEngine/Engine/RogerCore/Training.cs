/* 
RogerHubEgnine
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Education Algorithm v1.2.1
Written by Axolotl512

EducationWithTeacher, DropOut, multilayer, biases, RMS
*/
using RogerHubEngine.Client;
using RogerHubEngine.Engine.UI;

namespace RogerHubEngine.Engine.RogerCore
{
    /// <summary>
    /// Learning Algorithm Class
    /// </summary>
    public static class Training
    {
        /// <summary>
        /// Education Algorithm
        /// </summary>
        /// <param name="NN">A neural network that needs to be trained</param>
        /// <param name="param">Neural Network Parameters</param>
        /// <param name="status">An optional Progressbar object that will display how much the neural network has learned.</param>
        public static void Education(NeuralNetwork NN, Parameters param, Progressbar? status = null)
        {
            if (!NN.Initialized() || NN.educationArray == null)
                return;

            double progress = 0;
            bool done = false;
            object lockObj = new();

            const double eps = 1e-8;
            double grad;

            double[,] RMSinputWeights = null!;
            double[,] RMSoutputWeights = null!;
            double[][,] RMSmiddleWeights = null!;
            double[,] RMSmiddleBiases = null!;
            double[] RMSoutputBiases = null!;

            if (param.rms_enabled) //RMS cache
            {
                RMSinputWeights = new double[NN.inputWeights.GetLength(0), NN.inputWeights.GetLength(1)];
                RMSoutputWeights = new double[NN.outputWeights.GetLength(0), NN.outputWeights.GetLength(1)];
                RMSmiddleWeights = new double[NN.middleWeights.Length][,];
                RMSmiddleBiases = new double[NN.Mbias.GetLength(0), NN.Mbias.GetLength(1)];
                RMSoutputBiases = new double[NN.Obias.Length];

                Initialization.CreateMiddleWeights(RMSmiddleWeights, param.middleNeuronsCount);
            }

            int[] input = new int[NN.inputNeurons.Length];
            double[] output = new double[NN.outputNeurons.Length];
            double[,] errorMid = new double[NN.middleNeurons.GetLength(0), NN.middleNeurons.GetLength(1)];
            double[,] deltaMid = new double[NN.middleNeurons.GetLength(0), NN.middleNeurons.GetLength(1)];
            double[] errorOut = new double[NN.outputNeurons.Length];
            double[] deltaOut = new double[NN.outputNeurons.Length];
            double[,] oldOutputWeights = new double[NN.outputWeights.GetLength(0), NN.outputWeights.GetLength(1)];
            double[][,] oldMiddleWeights = new double[param.layers - 3][,];

            int nextLayer, oldLayer, lastMiddleWeights;

            if (param.layers > 3)
                lastMiddleWeights = NN.middleNeurons.GetLength(0) - 1;
            else
                lastMiddleWeights = 0;

            for (int x = 0; x < NN.middleWeights.Length; x++)
                oldMiddleWeights[x] = new double[NN.middleWeights[x].GetLength(0), NN.middleWeights[x].GetLength(1)];

            Thread uiThread = new(() =>
            {
                while (!done)
                {
                    double local;

                    lock (lockObj)
                        local = progress;

                    status?.Draw((int)(local * 100));

                    Thread.Sleep(1000);
                }
            })
            {
                IsBackground = true
            };
            uiThread.Start();

            for (int passes = 0; passes < param.passes; passes++)
            {
                for (int i = 0; i < NN.educationArray.GetLength(0); i++)
                {
                    for (int j = 0; j < input.Length; j++)
                        input[j] = Convert.ToInt32(NN.educationArray[i, j]);

                    Array.Clear(errorMid, 0, errorMid.Length);
                    Array.Clear(errorOut, 0, errorOut.Length);
                    Array.Clear(deltaMid, 0, deltaMid.Length);
                    Array.Clear(deltaOut, 0, deltaOut.Length);

                    for (int x = 0; x < NN.outputWeights.GetLength(0); x++)
                        for (int y = 0; y < NN.outputWeights.GetLength(1); y++)
                            oldOutputWeights[x, y] = NN.outputWeights[x, y];

                    for (int x = 0; x < oldMiddleWeights.Length; x++)
                    {
                        for (int y = 0; y < NN.middleWeights[x].GetLength(0); y++)
                            for (int z = 0; z < NN.middleWeights[x].GetLength(1); z++)
                                oldMiddleWeights[x][y, z] = NN.middleWeights[x][y, z];
                    }

                    float[,] dropOut = NN.GenerateDropOut();

                    //forward propagation
                    NeuralNetwork.ForwardPropagation(NN, input, dropOut);

                    for (int l = 0; l < NN.outputNeurons.Length; l++)
                        output[l] = NN.educationArray[i, l + NN.inputNeurons.Length];

                    for (int j = 0; j < NN.outputNeurons.Length; j++) //update output weights
                    {
                        errorOut[j] = NN.outputNeurons[j] - output[j]; //ошибка
                        deltaOut[j] = errorOut[j] * (1 - Math.Pow(NN.outputNeurons[j], 2.0)); //дельта

                        if (param.rms_enabled)
                        {
                            for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                            {
                                grad = NN.middleNeurons[lastMiddleWeights, k] * deltaOut[j];
                                RMSoutputWeights[k, j] = param.rms_decay * RMSoutputWeights[k, j] + (1.0 - param.rms_decay) * Math.Pow(grad, 2.0);

                                NN.outputWeights[k, j] -= param.learningRate * grad / (Math.Sqrt(RMSoutputWeights[k, j]) + eps);
                            }

                            RMSoutputBiases[j] = param.rms_decay * RMSoutputBiases[j] + (1.0 - param.rms_decay) * Math.Pow(deltaOut[j], 2.0);
                            NN.Obias[j] -= param.learningRate * deltaOut[j] / (Math.Sqrt(RMSoutputBiases[j]) + eps);
                        }
                        else
                        {
                            for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                NN.outputWeights[k, j] -= NN.middleNeurons[lastMiddleWeights, k] * deltaOut[j] * param.learningRate;

                            NN.Obias[j] -= deltaOut[j] * param.learningRate;
                        }
                    }

                    for (int j = 0; j < NN.middleNeurons.GetLength(1); j++) //update output->middle weights
                    {
                        int penultimateLayer = lastMiddleWeights - 1;
                        for (int l = 0; l < NN.outputNeurons.Length; l++)
                            errorMid[lastMiddleWeights, j] += deltaOut[l] * oldOutputWeights[j, l]; //ошибка

                        deltaMid[lastMiddleWeights, j] = errorMid[lastMiddleWeights, j] * (1 - Math.Pow(NN.middleNeurons[lastMiddleWeights, j], 2.0)); //дельта
                        deltaMid[lastMiddleWeights, j] *= dropOut[lastMiddleWeights, j];

                        if (param.rms_enabled)
                        {
                            if (param.layers - 2 > 1)
                                for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                {
                                    grad = NN.middleNeurons[penultimateLayer, k] * deltaMid[lastMiddleWeights, j];
                                    RMSmiddleWeights[penultimateLayer][k, j] = param.rms_decay * RMSmiddleWeights[penultimateLayer][k, j] +
                                        (1.0 - param.rms_decay) * Math.Pow(grad, 2.0);
                                    NN.middleWeights[penultimateLayer][k, j] -= param.learningRate * grad / (Math.Sqrt(RMSmiddleWeights[penultimateLayer][k, j]) + eps);
                                }
                            else
                                for (int k = 0; k < NN.inputNeurons.Length; k++)
                                {
                                    grad = input[k] * deltaMid[0, j];
                                    RMSinputWeights[k, j] = param.rms_decay * RMSinputWeights[k, j] + (1.0 - param.rms_decay) * Math.Pow(grad, 2.0);
                                    NN.inputWeights[k, j] -= param.learningRate * grad / (Math.Sqrt(RMSinputWeights[k, j]) + eps);
                                }

                            RMSmiddleBiases[lastMiddleWeights, j] = param.rms_decay * RMSmiddleBiases[lastMiddleWeights, j] + (1.0 - param.rms_decay) *
                                Math.Pow(deltaMid[lastMiddleWeights, j], 2.0);
                            NN.Mbias[lastMiddleWeights, j] -= param.learningRate * deltaMid[lastMiddleWeights, j] / (Math.Sqrt(RMSmiddleBiases[lastMiddleWeights, j]) + eps);
                        }
                        else
                        {
                            if (param.layers - 2 > 1)
                                for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                    NN.middleWeights[penultimateLayer][k, j] -=
                                        NN.middleNeurons[penultimateLayer, k] *
                                        deltaMid[lastMiddleWeights, j] *
                                        param.learningRate;
                            else
                                for (int k = 0; k < NN.inputNeurons.Length; k++)
                                    NN.inputWeights[k, j] -=
                                        input[k] *
                                        deltaMid[0, j] *
                                        param.learningRate;

                            NN.Mbias[lastMiddleWeights, j] -= deltaMid[lastMiddleWeights, j] * param.learningRate;
                        }
                    }

                    if (param.layers - 2 > 1)
                    {
                        for (int layer = param.layers - 4; layer >= 0; layer--) //update middle->middle weights
                        {
                            oldLayer = layer + 1;
                            nextLayer = layer - 1;

                            for (int j = 0; j < NN.middleNeurons.GetLength(1); j++)
                            {
                                for (int l = 0; l < NN.middleNeurons.GetLength(1); l++)
                                    errorMid[layer, j] += deltaMid[oldLayer, l] * oldMiddleWeights[layer][j, l]; //ошибка

                                deltaMid[layer, j] = errorMid[layer, j] * (1 - NN.middleNeurons[layer, j] * NN.middleNeurons[layer, j]); //дельта
                                deltaMid[layer, j] *= dropOut[layer, j];
                                if (param.rms_enabled)
                                {
                                    if (layer > 0)
                                        for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                        {
                                            grad = NN.middleNeurons[nextLayer, k] * deltaMid[layer, j];
                                            RMSmiddleWeights[nextLayer][k, j] = param.rms_decay * RMSmiddleWeights[nextLayer][k, j] + (1.0 - param.rms_decay) * Math.Pow(grad, 2.0);
                                            NN.middleWeights[nextLayer][k, j] -= param.learningRate * grad / (Math.Sqrt(RMSmiddleWeights[nextLayer][k, j]) + eps);
                                        }

                                    RMSmiddleBiases[layer, j] = param.rms_decay * RMSmiddleBiases[layer, j] + (1.0 - param.rms_decay) * Math.Pow(deltaMid[layer, j], 2.0);
                                    NN.Mbias[layer, j] -= param.learningRate * deltaMid[layer, j] / (Math.Sqrt(RMSmiddleBiases[layer, j]) + eps);
                                }

                                else
                                {
                                    if (layer > 0)
                                        for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                            NN.middleWeights[nextLayer][k, j] -= NN.middleNeurons[nextLayer, k] * deltaMid[layer, j] * param.learningRate;

                                    NN.Mbias[layer, j] -= deltaMid[layer, j] * param.learningRate;
                                }
                            }
                        }
                    }

                    if (param.layers - 2 > 1)
                    {
                        for (int j = 0; j < NN.inputNeurons.Length; j++) //update middle->input weights
                        {
                            if (param.rms_enabled)
                            {
                                for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                {
                                    grad = NN.inputNeurons[j] * deltaMid[0, k];
                                    RMSinputWeights[j, k] = param.rms_decay * RMSinputWeights[j, k] + (1.0 - param.rms_decay) * Math.Pow(grad, 2.0);
                                    NN.inputWeights[j, k] -= param.learningRate * grad / (Math.Sqrt(RMSinputWeights[j, k]) + eps);
                                }
                            }
                            else
                                for (int k = 0; k < NN.middleNeurons.GetLength(1); k++)
                                    NN.inputWeights[j, k] -= NN.inputNeurons[j] * deltaMid[0, k] * param.learningRate;
                        }
                    }

                    lock (lockObj)
                    {
                        progress =
                            (double)(passes * NN.educationArray.GetLength(0) + i + 1)
                            / (param.passes * NN.educationArray.GetLength(0));
                    }
                }
            }

            done = true;
            uiThread.Join();
        }
    }
}