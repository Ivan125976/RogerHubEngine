namespace RogerHubEngine.Engine.UI
{
    /// <summary>
    /// Creates beautiful menus and lists
    /// </summary>
    public class StringList
    {
        internal Queue<string> lines = new();

        /// <summary>
        /// List border style
        /// </summary>
        public BorderStyles borderStyle;

        internal int MaxLineLength { get; private set; } = 1;

        /// <summary>
        /// Adds a line to a list
        /// </summary>
        /// <param name="line">A line of text</param>
        public void Add(string line) => lines.Enqueue(line);

        /// <summary>
        /// Renders the list at the current cursor position
        /// </summary>
        public void Draw()
        {
            MaxLineLength = 1;

            int cursorX = Console.CursorLeft;

            foreach (var line in lines)
            {
                if (line.Length > MaxLineLength)
                    MaxLineLength = line.Length;
            }

            if (MaxLineLength > Console.WindowWidth)
                return;

            Console.CursorLeft = cursorX;

            if (borderStyle == BorderStyles.None)
            {
                foreach (var line in lines)
                {
                    Console.CursorLeft = cursorX;
                    Console.Write(line + Environment.NewLine);
                }
            }
            else
            {
                Console.Write(DrawBorder(BorderParts.Top) + Environment.NewLine);

                foreach (var line in lines)
                {
                    Console.CursorLeft = cursorX;
                    Console.Write(DrawBorder(BorderParts.Middle));
                    Console.Write(line + new string(' ', MaxLineLength - line.Length));
                    Console.Write(DrawBorder(BorderParts.Middle) + Environment.NewLine);
                }
                Console.CursorLeft = cursorX;
                Console.Write(DrawBorder(BorderParts.Bottom) + Environment.NewLine);
            }
        }

        private string? DrawBorder(BorderParts part)
        {
            return part switch
            {
                BorderParts.Top => borderStyle switch
                {
                    BorderStyles.StarsCorners => '*' + new string('-', MaxLineLength) + '*',
                    BorderStyles.RoundedCorners => '/' + new string('-', MaxLineLength) + '\\',
                    BorderStyles.FullyStars => new string('*', MaxLineLength + 2),
                    _ => null,
                },
                BorderParts.Middle => borderStyle switch
                {
                    BorderStyles.StarsCorners => ":",
                    BorderStyles.RoundedCorners => "|",
                    BorderStyles.FullyStars => "*",
                    _ => null,
                },
                BorderParts.Bottom => borderStyle switch
                {
                    BorderStyles.StarsCorners => '*' + new string('-', MaxLineLength) + '*',
                    BorderStyles.RoundedCorners => '\\' + new string('-', MaxLineLength) + '/',
                    BorderStyles.FullyStars => new string('*', MaxLineLength + 2),
                    _ => null,
                },
                _ => null,
            };
        }

        private enum BorderParts
        {
            Top,
            Middle,
            Bottom
        }

        /// <summary>
        /// Frame styles
        /// </summary>
        public enum BorderStyles
        {
            /// <summary>
            /// Without frame
            /// </summary>
            None,

            /// <summary>
            /// A simple border made of sticks, with stars at the corners
            /// </summary>
            StarsCorners,

            /// <summary>
            /// A border consisting entirely of sticks, with slashes at the corners that round them off
            /// </summary>
            RoundedCorners,

            /// <summary>
            /// A frame consisting entirely of stars
            /// </summary>
            FullyStars
        }
    }
}
