using IniParser;
using IniParser.Model;
using System.Globalization;
using System.Text.Json;
using Yocto_Roger.Yocto_Roger;
using static Yocto_Roger.UI.UI;

namespace Yocto_Roger.IO
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal I/O lib
*/
    internal static class MainIO
    {
        public static void SaveRoger()
        {
            using (StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "roger")))
            {
                Parameters param = new();
                writer.Write(
                    $"""
                 [roger]
                 AIversion = {RogerHubEngine.GetVersion(false)}
                 passes = {param.passes}
                 learningRate = {param.learningRate}
                 DropOutPercent = {param.DropOutPercent}

                 [neurons]
                 inputNeuronsCount = {param.inputNeuronsCount}
                 middleNeuronsCount = {param.middleNeuronsCount}
                 outputNeuronsCount = {param.outputNeuronsCount}

                 [layers]
                 Layers = {param.layers}
                 """);
            }
        }

        public static void SaveRogerToJson()
        {
            Parameters param = new();

            Roger roger = new()
            {
                AIversion = RogerHubEngine.GetVersion(false),
                Passes = param.passes,

                LearingRate = param.learningRate,
                DropOutPercent = param.DropOutPercent,

                InputNeuronsCount = param.inputNeuronsCount,
                MiddleNeuronsCount = param.middleNeuronsCount,
                OutputNeuronsCount = param.outputNeuronsCount,

                Layers = param.layers,
            };

            JsonSerializerOptions options = new() { WriteIndented = true };
            string jsonData = JsonSerializer.Serialize(roger, options);

            using (StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "json")))
                writer.Write(jsonData);
        }

        /// <summary>
        /// This method determines the type and parses Roger's file and provides an object in which all the data is in the form of strings, they will have to be converted to the required data types using the appropriate functions that seem to be in this class
        /// </summary>
        /// <returns>Roger class object. If happened any error, for example something with null, so it's returning an empty object of class Roger</returns>
        public static Roger? LoadRoger()
        {
            Parameters param = new();
            UI.UI user = new();

            if (!File.Exists(param.roger2))
                user.Send("Roger file not found", "error");
            else // I made an else clause so that if the file does not exist, the code will not be executed further.
            {
                switch (CheckFormat())
                {
                    case true: // Json
                        return LoadRogerFromJson();

                    case false: // Roger 
                        return LoadRogerFromRoger();
                }
            }
            return null; //It's a stub to keep the compiler from complaining. I have no idea how to fix it. I can fix it with GOTO, but damn... In theory, it's basically "unreachable code."
        }

        /// <summary>
        /// Checking the recording format
        /// </summary>
        /// <returns>true - json format, false - roger format</returns>
        private static bool? CheckFormat()
        {
            Parameters param = new();

            if (param.roger2.EndsWith(".json"))
                return true;
            else if (param.roger2.EndsWith(".roger") || param.roger2.EndsWith(".roger2"))
                return false;

            else return null;
        }

        /// <summary>
        /// A class that will store data for loading/saving the neural network.
        /// The data is stored as strings, so the data from it must be initialized using a special function InitRogersData, written specifically to avoid initializing everything manually.
        /// Please note, both when working with this class and simply when viewing it, that the absence of data is indicated by String.Empty instead of null!!!
        /// </summary>
        public class Roger
        {
            public string? AIversion { get; set; }

            public int Passes { get; set; }

            public float LearingRate { get; set; }
            public float DropOutPercent { get; set; }

            public int InputNeuronsCount { get; set; }
            public int MiddleNeuronsCount { get; set; }
            public int OutputNeuronsCount { get; set; }

            public int Layers { get; set; }
            public int MLayers { get; set; }
        }

        /// <summary>
        /// A function that returns a Roger class object with data extracted from a .roger file.
        /// </summary>
        private static Roger LoadRogerFromRoger()
        {
            Parameters param = new();

            var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(param.roger2);

                Roger roger = new()
                {
                    AIversion = data["roger"]["AIversion"],
                    Passes = Convert.ToInt32(data["roger"]["passes"]),
                    LearingRate = float.Parse(data["roger"]["learningRate"]),
                    DropOutPercent = float.Parse(data["roger"]["DropOutPercent"]),

                    InputNeuronsCount = Convert.ToInt32(data["neurons"]["inputNeuronsCount"]),
                    MiddleNeuronsCount = Convert.ToInt32(data["neurons"]["middleNeuronsCount"]),
                    OutputNeuronsCount = Convert.ToInt32(data["neurons"]["outputNeuronsCount"]),

                    Layers = Convert.ToInt32(data["layers"]["layers"]),
                    MLayers = Convert.ToInt32(data["layers"]["mLayers"])
                };

            return roger;
        }

        /// <summary>
        /// Returns an object of the Roger class with all the necessary data to load the neural network.
        /// </summary>
        private static Roger? LoadRogerFromJson()
        {
            Parameters param = new();

            using JsonDocument document = JsonDocument.Parse(File.ReadAllText(param.roger2));
            JsonElement root = document.RootElement;

            Roger roger = JsonSerializer.Deserialize<Roger>(File.ReadAllText(param.roger2));

            return roger ?? null;
        }

        /// <summary>
        /// Attempts to create a file in the same directory; if such a file already exists, it adds an index of attempts until it reaches the index where there is no file with that name.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="extension">File extension (without period)</param>
        public static string MakeFileSplitOnIndexIfExists(string filename, string extension)
        {
            int index = 0;

            string filenameWithIndex;
            do
            {
                if (index == 0)
                    filenameWithIndex = $"{filename}.{extension}";
                else
                    filenameWithIndex = $"{filename}{index}.{extension}";
                index++;
            }
            while (File.Exists(filenameWithIndex));

            FileStream fs = File.Create(filenameWithIndex);
            fs.Close();
            
            return filenameWithIndex;
        }

        //*************NEURAL NETWORK SECTION********************
        public class NeuralNetworkState
        {
            public string? EducationArray { get; set; }
            public string? InputNeurons { get; set; }
            public string? MiddleNeurons { get; set; }
            public string? OutputNeurons { get; set; }

            public int InputNeuronsCount { get; set; }
            public int MiddleNeuronsCount { get; set; }
            public int OutputNeuronsCount { get; set; }

            public string? InputWeights { get; set; }
            public string? MiddleWeights { get; set; }
            public string? OutputWeights { get; set; }

            public int Layers { get; set; }

            public string? Mbias { get; set; }
            public string? Obias { get; set; }
        }

        public static void InitNeuralNetwork(NeuralNetworkState nN, bool isNeededToInitEducationArray = false)
        {
            Parameters param = new();
            NeuralNetwork roger = new();
            if (isNeededToInitEducationArray)
                if (nN?.EducationArray is not null)
                    roger.educationArray = Auxiliary.ReadMatrixFromArray([.. nN.EducationArray.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture!))]);

            roger.inputNeurons = nN?.InputNeurons?.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            roger.middleNeurons = Auxiliary.ReadMatrixFromDoublesArray((nN?.MiddleNeurons is not null) ? [.. nN.MiddleNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            roger.outputNeurons = nN?.OutputNeurons?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            // Если null - значения по умолчанию
            param.inputNeuronsCount = nN?.InputNeuronsCount ?? 14;
            param.middleNeuronsCount = nN?.MiddleNeuronsCount ?? 16;
            param.outputNeuronsCount = nN?.OutputNeuronsCount ?? 8;

            roger.inputWeights = Auxiliary.ReadMatrixFromDoublesArray((nN?.InputWeights is not null) ? [.. nN.InputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            roger.middleWeights = Auxiliary.ReadJaggedMatrixFromArray((nN?.MiddleWeights is not null) ? [.. nN.MiddleWeights!.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null );
            roger.outputWeights = Auxiliary.ReadMatrixFromDoublesArray((nN?.OutputWeights is not null) ? [.. nN.OutputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);

            // Значения по умолчанию в случае null или пустого элемента
            param.layers = nN?.Layers ?? 3;

            roger.Mbias = Auxiliary.ReadMatrixFromDoublesArray((nN?.Mbias != null) ? [..nN.Mbias!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            roger.Obias = nN?.Obias?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();
                 
        }

        public static void SaveNeuralNetworkStateToJson(NeuralNetworkState nN, string pathToDirectoryToSave)
        {
            string json = JsonSerializer.Serialize(nN, new JsonSerializerOptions { WriteIndented = true} );

            string path = MakeFileSplitOnIndexIfExists(Path.Combine(pathToDirectoryToSave, "NeuralNetworkState"), "json");

            File.WriteAllText(path, json);
        }

        public static NeuralNetworkState FixTheStateOfNeuralNetwork(bool isNeedToFixTheEducationArray)
        {
            Parameters param = new();
            NeuralNetwork roger = new();
            NeuralNetworkState nN = new()
            {
                EducationArray = (isNeedToFixTheEducationArray) ? Auxiliary.BuildStringMatrix(roger.educationArray) ?? null : String.Empty,
                InputNeurons = Auxiliary.BuildStringArray(roger.inputNeurons) ?? null,
                MiddleNeurons = Auxiliary.BuildStringMatrix(roger.middleNeurons) ?? null,
                OutputNeurons = Auxiliary.BuildStringArray(roger.outputNeurons) ?? null,

                InputNeuronsCount = param.inputNeuronsCount,
                MiddleNeuronsCount = param.middleNeuronsCount,
                OutputNeuronsCount = param.outputNeuronsCount,

                InputWeights = Auxiliary.BuildStringArray(roger.inputWeights) ?? null,
                MiddleWeights = Auxiliary.BuildStringJaggedMatrix(roger.middleWeights) ?? null,
                OutputWeights = Auxiliary.BuildStringArray(roger.outputWeights) ?? null,

                Layers = param.layers,

                Obias = Auxiliary.BuildStringMatrix(roger.Mbias) ?? null,
                Mbias = Auxiliary.BuildStringArray(roger.Obias) ?? null,
            };

            return nN;
        }
        public static NeuralNetworkState LoadNeuralNetworkStateFromJson(string absolute_path)
        {
            NeuralNetworkState nN = new();

            using (JsonDocument doc = JsonDocument.Parse(File.ReadAllText(absolute_path)))
            {
                var root = doc.RootElement;

                nN.EducationArray = root.GetProperty("EducationArray").GetString() ?? null;

                nN.InputNeurons = root.GetProperty("InputNeurons").GetString() ?? null;
                nN.MiddleNeurons = root.GetProperty("MiddleNeurons").GetString() ?? null;
                nN.OutputNeurons = root.GetProperty("OutputNeurons").GetString() ?? null;

                nN.InputNeuronsCount = root.GetProperty("InputNeuronsCount").GetInt32();
                nN.MiddleNeuronsCount = root.GetProperty("MiddleNeuronsCount").GetInt32();
                nN.OutputNeuronsCount = root.GetProperty("OutputNeuronsCount").GetInt32();

                nN.InputWeights = root.GetProperty("InputWeights").GetString() ?? null;
                nN.MiddleWeights = root.GetProperty("MiddleWeights").GetString() ?? null;
                nN.OutputWeights = root.GetProperty("OutputWeights").GetString() ?? null;

                nN.Obias = root.GetProperty("Obias").GetString() ?? null;
                nN.Mbias = root.GetProperty("Mbias").GetString() ?? null;
            }

            return nN;
        }
    }
}
