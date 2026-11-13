/* 
    RogerHubEgnine
    *****************
    *Emotion Corp ;)*
    *****************
    Copyright 2025-2026 Emotion Corp.
*/

namespace RogerHubEngine.Engine.UI
{
    /// <summary>
    /// Draws a beautiful progress bar and can erase it automatically.
    /// </summary>
    public class Progressbar
    {
        private readonly ConsoleColor _color;
        private readonly int _parts;
        private readonly int _x;
        private readonly int _y;

        private readonly bool canDraw = false;

        /// <summary>
        /// Standart progressbar
        /// </summary>
        /// <param name="color">Color of the filled progress bar</param>
        /// <param name="parts">How many parts is the progress bar divided into</param>
        /// <param name="x">Abscissa</param>
        /// <param name="y">Ordinate</param>
        public Progressbar(ConsoleColor color, int parts, int x, int y)
        {
            _color = color;
            _parts = parts;
            _x = x;
            _y = y;
            canDraw = true;
        }

        /// <summary>
        /// Empty Progressbar
        /// </summary>
        public Progressbar() { }

        /// <summary>
        /// Draws the progressbar
        /// </summary>
        /// <param name="percent">Fill percentage</param>
        public void Draw(int percent)
        {
            if (!canDraw)
                return;

            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            Console.SetCursorPosition(_x, _y);
            Console.Write("[");
            Console.ForegroundColor = _color;
            int filled = Math.Clamp(percent * _parts / 100, 0, _parts);
            Console.Write(new string('█', filled));
            Console.Write(new string('░', _parts - filled));
            Console.ResetColor();
            Console.Write("]");
            Console.SetCursorPosition(cursorX, cursorY);
        }
    }
}
