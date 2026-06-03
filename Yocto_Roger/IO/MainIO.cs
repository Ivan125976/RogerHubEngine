using IniParser;
using IniParser.Model;
using System.Globalization;
using System.Text.Json;
using Yocto_Roger.Yocto_Roger;

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
    public class MainIO(Parameters param, NeuralNetwork nN, NeuralNetworkState nNState, UI.UI user, IO.Auxiliary auxiliaryIO)
    {
        private Parameters _param = param;
        public NeuralNetwork _nN = nN;
        private NeuralNetworkState _nNState = nNState;
        private UI.UI _user = user;
        private IO.Auxiliary _auxiliaryIO = auxiliaryIO;

        public void SaveRoger()
        {
            using (StreamWriter writer = new(MakeFileSplitOnIndexIfExists("roger", "roger")))
            {

                writer.Write(
                    $"""
                 [roger]
                 AIversion = {_param.version}
                 passes = {_param.passes}
                 learningRate = {_param.learningRate}
                 DropOutPercent = {_param.DropOutPercent}

                 [neurons]
                 inputNeuronsCount = {_param.inputNeuronsCount}
                 middleNeuronsCount = {_param.middleNeuronsCount}
                 outputNeuronsCount = {_param.outputNeuronsCount}

                 [layers]
                 Layers = {_param.layers}
                 """);
            }
        }

        public void SaveRogerToJson()
        {

            Roger roger = new()
            {
                AIversion = _param.version,
                Passes = _param.passes,

                LearingRate = _param.learningRate,
                DropOutPercent = _param.DropOutPercent,

                InputNeuronsCount = _param.inputNeuronsCount,
                MiddleNeuronsCount = _param.middleNeuronsCount,
                OutputNeuronsCount = _param.outputNeuronsCount,

                Layers = _param.layers,
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
        public Roger? LoadRoger()
        {

            if (!File.Exists(_param.roger2))
                _user.Send("Roger file not found", "error");
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
        private bool? CheckFormat()
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
        private Roger LoadRogerFromRoger()
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
        private Roger? LoadRogerFromJson()
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
        public string MakeFileSplitOnIndexIfExists(string filename, string extension)
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

        public void InitNeuralNetwork(NeuralNetworkState nN, bool isNeededToInitEducationArray = false)
        {

            //var any = RogerHubEngine.
            //NeuralNetwork roger = new();
            if (isNeededToInitEducationArray)
                if (nN?.EducationArray is not null)
                    _nN.educationArray = _auxiliaryIO.ReadMatrixFromArray([.. nN.EducationArray.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture!))]);

            _nN.inputNeurons = nN?.InputNeurons?.Split(';').Select(s => int.Parse(s, CultureInfo.InvariantCulture)).ToArray();
            _nN.middleNeurons = _auxiliaryIO.ReadMatrixFromDoublesArray((nN?.MiddleNeurons is not null) ? [.. nN.MiddleNeurons.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.outputNeurons = nN?.OutputNeurons?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

            // Если null - значения по умолчанию
            _param.inputNeuronsCount = nN?.InputNeuronsCount ?? 14;
            _param.middleNeuronsCount = nN?.MiddleNeuronsCount ?? 16;
            _param.outputNeuronsCount = nN?.OutputNeuronsCount ?? 8;

            _nN.inputWeights = _auxiliaryIO.ReadMatrixFromDoublesArray((nN?.InputWeights is not null) ? [.. nN.InputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.middleWeights = _auxiliaryIO.ReadJaggedMatrixFromArray((nN?.MiddleWeights is not null) ? [.. nN.MiddleWeights!.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.outputWeights = _auxiliaryIO.ReadMatrixFromDoublesArray((nN?.OutputWeights is not null) ? [.. nN.OutputWeights!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);

            // Значения по умолчанию в случае null или пустого элемента
            _param.layers = nN?.Layers ?? 3;

            _nN.Mbias = _auxiliaryIO.ReadMatrixFromDoublesArray((nN?.Mbias != null) ? [.. nN.Mbias!.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture))] : null);
            _nN.Obias = nN?.Obias?.Split(';').Select(s => double.Parse(s, CultureInfo.InvariantCulture)).ToArray();

        }

        public void SaveNeuralNetworkStateToJson(NeuralNetworkState nN, string pathToDirectoryToSave)
        {
            string json = JsonSerializer.Serialize(nN, new JsonSerializerOptions { WriteIndented = true });

            string path = MakeFileSplitOnIndexIfExists(Path.Combine(pathToDirectoryToSave, "NeuralNetworkState"), "json");

            File.WriteAllText(path, json);
        }

        public NeuralNetworkState FixTheStateOfNeuralNetwork(bool isNeedToFixTheEducationArray)
        {
            NeuralNetworkState nN = new()
            {
                EducationArray = (isNeedToFixTheEducationArray) ? _auxiliaryIO.BuildStringMatrix(_nN.educationArray) ?? null : String.Empty,
                InputNeurons = _auxiliaryIO.BuildStringArray(_nN.inputNeurons) ?? null,
                MiddleNeurons = _auxiliaryIO.BuildStringMatrix(_nN.middleNeurons) ?? null,
                OutputNeurons = _auxiliaryIO.BuildStringArray(_nN.outputNeurons) ?? null,

                InputNeuronsCount = _param.inputNeuronsCount,
                MiddleNeuronsCount = _param.middleNeuronsCount,
                OutputNeuronsCount = _param.outputNeuronsCount,

                InputWeights = _auxiliaryIO.BuildStringArray(_nN.inputWeights) ?? null,
                MiddleWeights = _auxiliaryIO.BuildStringJaggedMatrix(_nN.middleWeights) ?? null,
                OutputWeights = _auxiliaryIO.BuildStringArray(_nN.outputWeights) ?? null,

                Layers = _param.layers,

                Obias = _auxiliaryIO.BuildStringMatrix(_nN.Mbias) ?? null,
                Mbias = _auxiliaryIO.BuildStringArray(_nN.Obias) ?? null,
            };

            return nN;
        }
        public NeuralNetworkState LoadNeuralNetworkStateFromJson(string absolute_path)
        {
            using (JsonDocument doc = JsonDocument.Parse(File.ReadAllText(absolute_path)))
            {
                var root = doc.RootElement;

                _nNState.EducationArray = root.GetProperty("EducationArray").GetString() ?? null;

                _nNState.InputNeurons = root.GetProperty("InputNeurons").GetString() ?? null;
                _nNState.MiddleNeurons = root.GetProperty("MiddleNeurons").GetString() ?? null;
                _nNState.OutputNeurons = root.GetProperty("OutputNeurons").GetString() ?? null;

                _nNState.InputNeuronsCount = root.GetProperty("InputNeuronsCount").GetInt32();
                _nNState.MiddleNeuronsCount = root.GetProperty("MiddleNeuronsCount").GetInt32();
                _nNState.OutputNeuronsCount = root.GetProperty("OutputNeuronsCount").GetInt32();

                _nNState.InputWeights = root.GetProperty("InputWeights").GetString() ?? null;
                _nNState.MiddleWeights = root.GetProperty("MiddleWeights").GetString() ?? null;
                _nNState.OutputWeights = root.GetProperty("OutputWeights").GetString() ?? null;

                _nNState.Obias = root.GetProperty("Obias").GetString() ?? null;
                _nNState.Mbias = root.GetProperty("Mbias").GetString() ?? null;
            }

            return _nNState;
        }
    }
}
