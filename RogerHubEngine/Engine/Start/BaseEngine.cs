using RogerHubEngine.Engine.Graphics;
using System.Text;

namespace RogerHubEngine.Engine.Start
{
    class BaseEngine
    {
        public static void Main()
        {
            ConsoleConfig data = new("RogerHubEngine 3", Encoding.Unicode, 200, 50, false);
            BaseConsole.Configure(data);

            Console.WriteLine("This project still in the development stage. And this is a BETA fersion of new RogerHubEngine 3. Having fun!");
            Thread.Sleep(3000);

            TUI.SrartTUI(1);

            for (int x = 0; x < Console.WindowWidth; x++)
                for (int y = 0; y < Console.WindowHeight; y++)
                    TUI.SetPixel(x, y, '#');
        }
    }
}