using Yocto_Roger.Client.Interfaces;
using Yocto_Roger.RogerCore;

namespace Yocto_Roger.Client
{
    internal class Initialization
    {
        public static MainMenuInterface Init()
        {
            Parameters param = new();
            IO io = new(param, null!);
            SettingsInterface settingsInterface = new(param, io);
            MainMenuInterface mainMenuInterface = new(settingsInterface, null!);
            NeuralNetworkInterface neuralNetworkInterface = new(io, mainMenuInterface, null!);
            Training training = new(param, null!);
            NeuralNetwork nN = new(param, io, training, neuralNetworkInterface, mainMenuInterface);

            io._nN = nN;
            training.roger = nN;
            mainMenuInterface._roger = nN;
            neuralNetworkInterface._neuralNetwork = nN;

            return mainMenuInterface;
        }
    }
}
