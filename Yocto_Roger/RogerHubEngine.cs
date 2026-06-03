using Yocto_Roger.IO;
using Yocto_Roger.UI;
using Yocto_Roger.Yocto_Roger;

namespace Yocto_Roger
{

    public class RogerHubEngine
    {
        //public RogerHubEngine(UI.UI user, Parameters param, NeuralNetwork nN)
        //{
        //    _user = user;
        //    _param = param;
        //    _nN = nN;
        //}

        static public void Main()
        {
            Parameters param = new();
            NeuralNetworkState nNState = new();

            UI.UI user = new(null!, null!, param);
            IO.Auxiliary auxiliaryIO = new(param);
            MainIO io = new(param, null!, nNState, user, auxiliaryIO);
            Weights weights = new(param, user);
            Biases biases = new(param, user);
            AIMath aiMath = new(param);
            Training training = new(param, null!, aiMath);
            NeuralNetwork nN = new(param, user, nNState, io, weights, biases, training, aiMath);
            SetUpInterface setUpInterface = new(param, user, io, auxiliaryIO);

            // Инициализация недостающих элементов, которые в конструкторах null, ибо они образовывали замкнутый круг когда чтобы для класса нужен объект, а для этого объекта нужен класс для которого нужен объект, короче забей, работает  - главное
            io._nN = nN;
            user._nN = nN;
            user._setUpInterface = setUpInterface;
            training._nN = nN;

            user.Start();
        }
    }
}
