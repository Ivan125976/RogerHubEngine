using System.Text;
using System.Drawing;
using Yocto_Roger.RogerCore;
using static Yocto_Roger.Client.EngineVersion;
using static Yocto_Roger.UI.CUI;
using Yocto_Roger.Client.Interfaces;
using Yocto_Roger.UI;

namespace Yocto_Roger.Client
{

    /* 
     We are not idiots.
     We are idiots++.
     © Emotion Corp.
    */

    /// <summary>
    /// Main class
    /// </summary>
    public class RogerHubEngine
    {
        /// <summary>
        /// A class that creates the environment and starts RogerHubEngine
        /// </summary>
        static public void Main()
        {
            Size ConsoleSize = new(40, 120);

            Console.Clear();
            ASCIIDraw.Logo(true);
            Console.WriteLine("Creating a software environment...");
            if (!CheckMinWindowSize(ConsoleSize))
            {
                if (OperatingSystem.IsWindows())
                {
                    try
                    {
                        Console.SetWindowSize(width: ConsoleSize.Width, height: ConsoleSize.Height);
                    }
                    catch
                    {
                        Console.Write($"\x1b[8;{ConsoleSize.Width};{ConsoleSize.Height}t");

                        Thread.Sleep(25); // Delay to allow time for the size to change

                        if (!CheckMinWindowSize(ConsoleSize)) // If escape-code didn't work
                        {
                            Send($"Unable to resize the console. You'll have to do it yourself :( \nneed: \nWidth: {ConsoleSize.Width} \nHeight: {ConsoleSize.Height}", MessageType.error);
                            Send("Resize the window until i say \"Done\"", MessageType.note);
                            while (!CheckMinWindowSize(ConsoleSize))
                            {
                                bool check = CheckMinWindowSize(ConsoleSize);

                                if (check) { Send("Done"); }
                            }
                        }
                    }
                }
                else
                {
                    Console.Write("\n");
                    Console.Write($"\x1b[8;{ConsoleSize.Height};{ConsoleSize.Width}t");

                    Thread.Sleep(25); // Delay to allow time for the size to change

                    if (!CheckMinWindowSize(ConsoleSize)) // If escape-code didn't work
                    {
                        Send($"Unable to resize the console. You'll have to do it yourself :( \nneed: \nWidth: {ConsoleSize.Width} \nHeight: {ConsoleSize.Height}", MessageType.error);
                        Send("Resize the window until I say \"Done\"", MessageType.note);
                        while (!CheckMinWindowSize(ConsoleSize))
                        {
                            bool check = CheckMinWindowSize(ConsoleSize);

                            if (check) { Send("Done"); }
                        }
                    }

                }
            }


            Console.Title = $"RogerHubEngine v{majorVersion}.{minorVersion}.{patchVersion}";

            // Some terminals (mostly on GNU/Linux) don't support Unicode, and throwing exception, but supporting UTF-8
            try
            {
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;
            }
            catch
            {
                InternalError("Your system doesn't support Unicode!");
            }

            MainMenuInterface mainMenuInterface = Initialization.Init();

            DrawLine(ConsoleColor.Magenta, "Emotion ;) 2025-2026", "Roger :D");
            Thread.Sleep(3000);

            mainMenuInterface.StartInterface();
        }

        /// <summary>
        /// If Console Size > ConsoleSize - true. Else - false
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool CheckMinWindowSize(Size size) => Console.WindowWidth > size.Width && Console.WindowHeight > size.Height;
    }
}
