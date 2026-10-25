using RogerHubEngine.Engine.UI;

namespace RogerHubEngine.Client.Interfaces
{
    internal class RRNNsInterface
    {
        public static void StartInterface()
        {
            ConsoleManager.Clear();

            CUI.DrawLine(ConsoleColor.DarkBlue, "RRNNs Compilator");
            Console.WriteLine("This app isn't ready");

            Console.ReadKey();
        }
    }
}
