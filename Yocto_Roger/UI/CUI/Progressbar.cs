namespace Yocto_Roger.UI.CUI
{
    /// <summary>
    /// Draws a beautiful progress bar and can erase it automatically.
    /// </summary>
    /// <param name="color">Color of the filled progress bar</param>
    /// <param name="parts">How many parts is the progress bar divided into</param>
    /// <param name="x">Abscissa</param>
    /// <param name="y">Ordinate</param>

    public class Progressbar(ConsoleColor color, int parts, int x, int y)
    {
        private readonly int _parts = parts;
        private readonly ConsoleColor _color = color;
        private readonly int _x = x;
        private readonly int _y = y;

        /// <summary>
        /// Redraws the progressbar
        /// </summary>
        /// <param name="percent">Fill percentage</param>
        public void Draw(int percent) //Ivan: Очень плохо использовать Thread, для прогрессбара, лучше сделайть этот метод ассинхронный и вызывай его с помощью await, это намного лучше, и читается лучше
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            Console.SetCursorPosition(_x + _parts + 2, _y);
            Console.Write(new string('\b', _parts + 2));
            Console.Write("[");
            Console.ForegroundColor = _color;
            int filled = Math.Clamp(percent * _parts / 100, 0, _parts);
            Console.Write(new string('█', filled));
            Console.Write(new string('░', _parts - filled));
            Console.ResetColor();
            Console.Write("]");
            Console.SetCursorPosition(cursorX, cursorY);
        }

        /// <summary>
        /// Clears the progress bar
        /// </summary>
        public void Remove()
        {
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;
            Console.SetCursorPosition(_x + _parts + 2, _y);
            Console.Write(new string('\b', _parts + 2));
            Console.SetCursorPosition(cursorX, cursorY);
        }
    }
}
