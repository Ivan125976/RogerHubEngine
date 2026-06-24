using System.Text;

namespace RogerHubEngine.Engine.Start
{
    internal class BaseConsole
    {
        public static void Configure(ConsoleConfig data)
        {
            Console.Title = data.Title;

            try { Console.InputEncoding = data.Encoding; } catch { Console.InputEncoding = Encoding.UTF8; }
            try { Console.OutputEncoding = data.Encoding; } catch { Console.OutputEncoding = Encoding.UTF8; }

            if (data.Width > 0 && data.Height > 0)
                Console.Write($"\x1b[8;{data.Height};{data.Width}t");

            Console.CursorVisible = data.CursorVisible;
        }
    }
}
