using MemoryPack;
using System.Text.Json;
using RogerHubEngine.Client;
using RogerHubEngine.Engine.UI;
using RogerHubEngine.RogerCore;
using static RogerHubEngine.Client.EngineVersion;
using static RogerHubEngine.Engine.UI.CUI;

namespace RogerHubEngine.Engine.RogerCore
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal I/O lib
*/
    /// <summary>
    /// Main IO class, where contains all main function for work with salve/load
    /// </summary>
    public class IO
    {
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Attempts to create a file in the same directory; if such a file already exists, it adds an index of attempts until it reaches the index where there is no file with that name.
        /// </summary>
        /// <param name="fileName">Can be path + filename. Example: C:\Users\Noob\Desktop\roger, where roger is filename, without extension. Then file will be created in Desktop</param>
        /// <param name="extension">File extension (without period)</param>
        /// <exception cref="DirectoryNotFoundException">When directory, which you entered, doesn't exist</exception>
        /// <exception cref="UnauthorizedAccessException">This exception occurs when there are insufficient rights to create files in a particular directory. For example: The program does not have administrator rights, but the path specified is C:\Windows\System32, which requires administrator rights to write to this directory. </exception>
        /// <exception cref="IOException"></exception>
        public static string MakeFileSplitOnIndexIfExists(string extension, string? fileName = "RogerFile")
        {
            int index = 0;

            string filenameWithIndex;
            do
            {
                if (index == 0)
                    filenameWithIndex = $"{fileName}.{extension}";
                else
                    filenameWithIndex = $"{fileName}{index}.{extension}";
                index++;
            }
            while (File.Exists(filenameWithIndex));

            FileStream fs = File.Create(filenameWithIndex);
            fs.Close();

            return filenameWithIndex;
        }

        /// <summary>
        /// Saving the current Roger settings in the json file, which creating automatedly
        /// </summary>
        public static void SaveRogerToJson(string? fileName, Parameters param)
        {
            Roger roger = new()
            {
                Version = $"{engineVersion.Major}.{engineVersion.Minor}",
                AIparams = param
            };

            string jsonData = JsonSerializer.Serialize(roger, options);

            using StreamWriter writer = new(MakeFileSplitOnIndexIfExists("params", fileName));
            writer.Write(jsonData);
        }

        /// <summary>
        /// This method determines the type and parses Roger's file and provides an object in which all the data is in the form of strings, they will have to be converted to the required data types using the appropriate functions that seem to be in this class
        /// This method calls function SEND, with error, which outputs text to the console. Be careful, with this method when you will using it in your project. replace SEND call, on exception throwing
        /// </summary>
        /// <returns>Roger class object. If happened any error, for example something with null, so it's returning an empty object of class Roger</returns>
        public static Roger? LoadRoger(Parameters param)
        {
            try
            {
                if (LoadRogerFromJson(param) is Roger roger)
                    return roger;
                else
                    return null;
            }
            catch (JsonException e)
            {
                Send($"Failed to parse the json data: \n{e}", MessageType.error);
                return null;
            }
        }

        /// <summary>
        /// A class that will store data for loading/saving the neural network.
        /// The data is stored as strings, so the data from it must be initialized using a special function InitRogersData, written specifically to avoid initializing everything manually.
        /// Please note, both when working with this class and simply when viewing it, that the absence of data is indicated by String.Empty instead of null!!!
        /// </summary>
        public struct Roger
        {
            /// <summary>
            /// Engine version
            /// </summary>
            public required string Version { get; set; }

            /// <summary>
            /// AI parameters
            /// </summary>
            public required Parameters AIparams { get; set; }
        }

        /// <summary>
        /// Returns an object of the Roger class with all the necessary data to load the neural network.
        /// </summary>
        /// <exception cref="ArgumentNullException">This Exception throwing, when file which it parsing (params.roger2), is null or empty, meaning it's doesn't exists</exception>
        /// <exception cref="JsonException">This exception is thrown when the text cannot be serialized into json format, meaning that the file being parsed contains strange text that is not actually suitable for serialization into json.</exception>
        private static Roger? LoadRogerFromJson(Parameters param)
        {
            Roger? roger = JsonSerializer.Deserialize<Roger>(File.ReadAllText(param.roger2));
            return roger ?? null;
        }

        /// <summary>
        /// Transforming the values from class NeuralNetworkState to needed types, and initializing it where it needs
        /// </summary>
        /// <param name="roger">Your object that you need to initialize</param>
        /// <param name="mainClassLink">Your class in which roger will be initialized</param>
        /// <param name="paramClassLink">Link to main Parameters class in which Layers from roger object will be initialized</param>
        public static void InitNeuralNetwork(NeuralNetwork roger, NeuralNetwork mainClassLink, Parameters paramClassLink)
        {
            // If null - Values by default
            mainClassLink.inputNeurons = roger?.inputNeurons;
            mainClassLink.middleNeurons = roger?.middleNeurons;
            mainClassLink.outputNeurons = roger?.outputNeurons;

            mainClassLink.inputWeights = roger?.inputWeights!;
            mainClassLink.middleWeights = roger?.middleWeights ?? null; // Can be null and more likely, will be null anyway, i guess
            mainClassLink.outputWeights = roger?.outputWeights!;

            // If null - values by default
            //paramClassLink.layers = roger?.Layers ?? 3;

            mainClassLink.Mbias = roger?.Mbias;
            mainClassLink.Obias = roger?.Obias;

        }
        /// <summary>
        /// Saving neural network state to binary file. This function savin' files only in the current directory!!!
        /// </summary>
        /// <param name="nN"></param>
        /// <param name="fileName">Not a path, 'cause the</param>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="MemoryPackSerializationException"></exception>
        public static void SaveNeuralNetworkStateToBin(NeuralNetwork nN, string fileName = "RogerFile")
        {

            byte[] binData = MemoryPackSerializer.Serialize(nN);

            string path = MakeFileSplitOnIndexIfExists("roger2", Path.Combine(Directory.GetCurrentDirectory(), fileName ?? "RogerFile"));

            File.WriteAllBytes(path, binData);
        }
        /// <summary>
        /// Fixing the neural network state
        /// </summary>
        /// <param name="linkToMainClass">link to the main class from which parameters will be obtained.</param>
        /// <param name="linkToParameters">link to the main class from which parameters will be obtained</param>
        /// <returns></returns>
        public static NeuralNetwork FixTheStateOfNeuralNetwork(NeuralNetwork linkToMainClass, Parameters linkToParametersClass)
        {
            // just fo comfort
            var ltmc = linkToMainClass;
            var ltp = linkToParametersClass;
            NeuralNetwork nN = new(ltp)
            {

                // If it's null, then it automatedly set to default value
                inputNeurons = ltmc?.inputNeurons!,
                middleNeurons = ltmc?.middleNeurons!,
                outputNeurons = ltmc?.outputNeurons!,

                inputWeights = ltmc?.inputWeights ?? null,
                middleWeights = ltmc?.middleWeights ?? null,
                outputWeights = ltmc?.outputWeights ?? null,

                // And here too
                //layers = ltp.layers,

                Obias = ltmc?.Obias ?? null,
                Mbias = ltmc?.Mbias ?? null
            };

            return nN;
        }

        /// <summary>
        /// Loading values from the file 
        /// </summary>
        /// <param name="absolute_path">Absolute path to the file</param>
        /// <returns></returns>
        public static NeuralNetwork? LoadNeuralNetworkStateFromBin(string absolute_path)
        {
            NeuralNetwork? nNState = MemoryPackSerializer.Deserialize<NeuralNetwork>(File.ReadAllBytes(absolute_path));

            return nNState;
        }

        /// <summary>
        /// Converts data from the passed object into the appropriate types and initializes the corresponding variables.
        /// </summary>
        /// <param name="roger"></param>
        /// <param name="param"></param>
        public static void LoadSettingsFile(Roger roger, Parameters param)
        {
            if (roger.Version == $"{engineVersion.Major}.{engineVersion.Minor}")
            {
                param.passes = roger.AIparams.passes;
                param.learningRate = roger.AIparams.learningRate;
                param.DropOutPercent = roger.AIparams.DropOutPercent;

                param.knowledgeFile = roger.AIparams.knowledgeFile;

                param.middleNeuronsCount = roger.AIparams.middleNeuronsCount;

                param.layers = roger.AIparams.layers;

                param.rms_enabled = roger.AIparams.rms_enabled;
                param.rms_decay = roger.AIparams.rms_decay;
            }
            else
                Send($"Your settings file is intended for a different version! ({roger.Version})", MessageType.error);
            //TODO: Функцию конвертирования
        }

        /// <summary>
        /// Checks whether the user-provided file has the correct extension.
        /// </summary>
        /// <param name="input">User input</param>
        /// <param name="extension">The extension the file should have</param>
        /// <returns>File name with extension, or null if the file does not exist.</returns>
        public static string? FileExtensionCheck(string input, string extension)
        {
            if (File.Exists(input)) return input;
            else if (File.Exists(input + extension)) return input + extension;
            else return null;
        }

        /// <summary>
        /// Splits a string using the specified character.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <param name="splitChar">The character that will divide the string</param>
        /// <returns></returns>
        public static string[] SplitString(string input, char splitChar) => input.Split(splitChar);

        /// <summary>
        /// Splitting a string into parts using a symbol
        /// </summary>
        /// <param name="obj">The string to be splitted</param>
        /// <returns></returns>
        public static int[] StringToIntArray(string[] obj)
        {
            int[] parsedArray = new int[obj.Length];
            for (int i = 0; i < parsedArray.Length; i++)
                parsedArray[i] = Convert.ToInt32(obj[i]);
            return parsedArray;
        }
    }
}
