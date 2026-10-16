/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

using System.Drawing;

namespace RogerHubEngine.Client
{

    internal class ConsoleManager
    {
        public static ConsoleColor defaultBackgroundColor = ConsoleColor.Black;
        public static ConsoleColor defaultForegroundColor = ConsoleColor.Gray;

        public static void ResizeConsole(int width, int height)
        {
            Console.Write($"\x1b[8;{height};{width}t");
        }

        public static void ResizeConsole(Size consoleSize)
        {
            int width = consoleSize.Width;
            int height = consoleSize.Height;

            Console.Write($"\x1b[8;{height};{width}t");
        }

        public static void ResetColor()
        {
            Console.BackgroundColor = defaultBackgroundColor;
            Console.ForegroundColor = defaultForegroundColor;
        }
    }
}
