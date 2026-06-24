using System.Text;

namespace RogerHubEngine.Engine.Start
{
    class BaseEngine
    {
        public static void Main()
        {
            ConsoleConfig data = new("RogerHubEngine 3", Encoding.Unicode, 200, 50, false);

            BaseConsole.Configure(data);

            Console.ReadLine();
        }
    }
}