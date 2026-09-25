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
    public class IO : IDisposable
    {
        private static readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Saving the current Roger settings in the json file, which creating automatedly
        /// </summary>
        public static void SaveRogerToJson(string? fileName, Parameters param)
        {
            Roger roger = new()
            {
                AIversion = $"{majorVersion}.{minorVersion}",
                Passes = param.passes,

                KnowledgeFile = param.knowledgeFile,

                LearingRate = param.learningRate,
                DropOutPercent = param.DropOutPercent,

                MiddleNeuronsCount = param.middleNeuronsCount,

                Layers = param.layers,

                Rms_decay = param.rms_decay,

                Rms_enabled = param.rms_enabled
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
        public class Roger
        {
            /// <summary>
            /// Ai version
            /// </summary>
            public string? AIversion { get; set; }

            /// <summary>
            /// Knowledge.know file
            /// </summary>
            public string? KnowledgeFile { get; set; }
            /// <summary>
            /// Passes
            /// </summary>
            public int Passes { get; set; }

            /// <summary>
            /// Learning rate
            /// </summary>
            public float LearingRate { get; set; }
            /// <summary>
            /// Drop out percent
            /// </summary>
            public float DropOutPercent { get; set; }

            /// <summary>
            /// Middle neurons count
            /// </summary>
            public int MiddleNeuronsCount { get; set; }

            /// <summary>
            /// Layers
            /// </summary>
            public int Layers { get; set; }
            /// <summary>
            /// MLayers
            /// </summary>
            public int MLayers { get; set; }

            /// <summary>
            /// Rms_Decay. I don't know what it is, Ivan knows, asking him
            /// </summary>
            public float Rms_decay { get; set; }

            /// <summary>
            /// is RMS enabled
            /// </summary>
            public bool Rms_enabled { get; set; }
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
        /// Transforming the values from class NeuralNetworkState to needed types, and initializing it where it needs
        /// </summary>
        /// <param name="nN"></param>
        /// <param name="param"></param>
        public static void InitNeuralNetwork(NeuralNetwork Roger, Parameters param)
        {
            // If null - Values by default
            nN.inputNeurons = nN?.InputNeurons;
            nN.middleNeurons = nN?.MiddleNeurons;
            nN.outputNeurons = nN?.OutputNeurons;

            nN.inputWeights = nN?.InputWeights!;
            nN.middleWeights = nN?.MiddleWeights ?? null; // Can be null and more likely, will be null anyway, i guess
            nN.outputWeights = nN?.OutputWeights!;

            // If null - values by default
            param.layers = nN?.Layers ?? 3;

            nN.Mbias = nN?.Mbias;
            nN.Obias = nN?.Obias;

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
        public static void SaveNeuralNetworkStateToBin(NeuralNetworkState nN, string fileName = "RogerFile")
        {

            byte[] binData = MemoryPackSerializer.Serialize(nN);

            string path = MakeFileSplitOnIndexIfExists("roger2", Path.Combine(Directory.GetCurrentDirectory(), fileName ?? "RogerFile"));

            File.WriteAllBytes(path, binData);
        }
        /// <summary>
        /// Fixing the neural network state
        /// </summary>
        /// <returns></returns>
        public static NeuralNetworkState FixTheStateOfNeuralNetwork()
        {
            NeuralNetworkState nN = new()
            {

                // If it's null, then it automatedly set to default value
                InputNeurons = _nN?.inputNeurons!,
                MiddleNeurons = _nN?.middleNeurons!,
                OutputNeurons = _nN?.outputNeurons!,

                InputWeights = _nN?.inputWeights ?? null,
                MiddleWeights = _nN?.middleWeights ?? null,
                OutputWeights = _nN?.outputWeights ?? null,

                // And here too
                Layers = _param.layers,

                Obias = _nN?.Obias ?? null,
                Mbias = _nN?.Mbias ?? null
            };

            return nN;
        }

        /// <summary>
        /// Loading values from the file 
        /// </summary>
        /// <param name="absolute_path">Absolute path to the file</param>
        /// <returns></returns>
        public static NeuralNetworkState? LoadNeuralNetworkStateFromBin(string absolute_path)
        {
            NeuralNetworkState? nNState = MemoryPackSerializer.Deserialize<NeuralNetworkState>(File.ReadAllBytes(absolute_path));

            return nNState;
        }

        /// <summary>
        /// Converts data (strings) from the passed object into the appropriate types and initializes the corresponding variables. If null is passed, it initializes default values.
        /// </summary>
        /// <param name="roger"></param>
        public static void InitRogersData(Roger? roger, Parameters param)
        {
            if (roger?.AIversion == $"{majorVersion}.{minorVersion}")
            {
                param.passes = roger?.Passes ?? 10000;
                param.learningRate = roger?.LearingRate ?? 0.01f;
                param.DropOutPercent = roger?.DropOutPercent ?? 8.0f;

                param.knowledgeFile = roger?.KnowledgeFile ?? string.Empty;

                param.middleNeuronsCount = roger?.MiddleNeuronsCount ?? 16;

                param.layers = roger?.Layers ?? 4;

                param.rms_enabled = roger?.Rms_enabled ?? false;
                param.rms_decay = roger?.Rms_decay ?? 0.95f;
            }
            else
                Send($"Your settings file is intended for a different version! ({roger?.AIversion})", MessageType.error);
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

        public void Dispose()
        {
            //TODO: Сделать Dispose метод
            //GC.SuppressFinalize(this);
        }
    }
}
