namespace Yocto_Roger.UI.CUI
/* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
    RogerHub UI part
*/

{
    /// <summary>
    /// Internal library for a beautiful command line
    /// </summary>
    public class CUI()
    {
        /// <summary>
        /// Draws a stripe of a specified color at the bottom of the console window with auto-text color.
        /// </summary>
        /// <param name="color">Background text color</param>
        /// <param name="leftText">Left text</param>
        /// <param name="rightText">Right text</param>
        public static void DrawLine(ConsoleColor color, string leftText = "", string rightText = "")
        {
            Console.ForegroundColor = color switch
            {
                ConsoleColor.Gray or ConsoleColor.White or ConsoleColor.Yellow or ConsoleColor.DarkYellow or ConsoleColor.Cyan or ConsoleColor.Green or ConsoleColor.DarkGreen => ConsoleColor.Black,
                _ => ConsoleColor.White,
            };
            int cursorX = Console.CursorLeft;
            int cursorY = Console.CursorTop;

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.BackgroundColor = color;

            Console.Write(new string(' ', Console.WindowWidth));

            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write(leftText);

            Console.SetCursorPosition(Console.WindowWidth - rightText.Length - 1, Console.WindowHeight - 1);
            Console.Write(rightText);

            Console.SetCursorPosition(cursorX, cursorY);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
        }



        /// <summary>
        /// Draws a beautiful message to the user about something
        /// </summary>
        /// <param name="message">Message text</param>
        /// <param name="mode">The color and meaning of the message will depend on the mode. Available modes are "error," "warning," and "message." The default mode is "message."</param>
        public static void Send(string message, MessageType mode = MessageType.message)
        {
            switch (mode)
            {
                case MessageType.note:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(message);
                    Console.ResetColor();
                    break;

                case MessageType.message:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(message + "\n");
                    Console.ResetColor();
                    break;

                case MessageType.warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("WARNING>" + message);
                    Console.ResetColor();
                    break;

                case MessageType.error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR>" + message);
                    Console.ResetColor();
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey(true);
                    break;

                default:
                    Send("UI.Send>Incorrect mode! Check the UI.Send method call", MessageType.error);
                    break;
            }
        }
    }
}
