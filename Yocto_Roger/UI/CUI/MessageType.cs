namespace Yocto_Roger.UI.CUI
{
    /// <summary>
    /// Message type
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Draws the message in blue
        /// </summary>
        note,

        /// <summary>
        /// Draws the message in green, then advances one line
        /// </summary>
        message,

        /// <summary>
        /// Draws a message in yellow, marked WARNING
        /// </summary>
        warning,

        /// <summary>
        /// It displays an error message in red, marked "ERROR," and then prompts the user to press a key
        /// </summary>
        error
    }
}
