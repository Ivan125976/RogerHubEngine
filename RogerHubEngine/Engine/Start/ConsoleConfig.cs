using System.Text;

namespace RogerHubEngine.Engine.Start
{
    internal class ConsoleConfig(string? title, Encoding? encoding, int width, int height, bool cursorVisible)
    {
        public string Title { get; private set; } = title ?? "New RogerHubEngine window";
        public Encoding Encoding { get; private set; } = encoding ?? Encoding.UTF8;
        public int Width { get; private set; } = width;
        public int Height { get; private set; } = height;
        public bool CursorVisible { get; private set; } = cursorVisible;
    }
}
