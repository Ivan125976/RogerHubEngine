/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

using RogerHubEngine.Client.Interfaces;
using RogerHubEngine.Engine.UI;
using System.Drawing;
using System.Text;
using static RogerHubEngine.Client.EngineVersion;
using static RogerHubEngine.Engine.UI.CUI;

namespace RogerHubEngine.Client
{
    /// <summary>
    /// Main class
    /// </summary>
    public class RogerHubEngine()
    {
        /// <summary>
        /// A class that creates the environment and starts RogerHubEngine
        /// </summary>
        static public void Main()
        {
            Size ConsoleSize = new(120, 40);

            Console.Clear();
            ASCIIDraw.Logo(true);
            Console.WriteLine("Creating a software environment...");

            ConsoleManager.ResizeConsole(ConsoleSize);

            Console.Title = $"RogerHubEngine v{engineVersion.Major}.{engineVersion.Minor}.{engineVersion.Build}";

            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2025-2026", "Roger :D");
            Thread.Sleep(3000);

            MainMenuInterface.StartInterface();
        }
    }
}
