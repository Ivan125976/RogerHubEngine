using System.Text.RegularExpressions;

namespace Yocto_Roger.RogerCore.UtilityTools
{
    /* 
Yocto Roger ;)
*****************
*Emotion Corp ;)*
*****************
Copyright 2025-2026 Emotion Corp.
Internal AIMath lib
*/

    /// <summary>
    /// Internal Math lib for Roger
    /// </summary>
    public partial class RogerMath(Parameters param)
    {
        private readonly Parameters _param = param;

        /// <summary>
        /// System Random
        /// </summary>
        public static readonly Random rand = new();

        /// <summary>
        /// Tanh Activation
        /// </summary>
        /// <param name="value">Value</param>
        public static double Tanh(double value)
        {
            return Math.Tanh(value);
        }

        /// <summary>
        /// Cleanses the number from all otherworldly symbols
        /// </summary>
        /// <param name="input">String with numbers</param>
        /// <param name="cleaned">Cleaned line</param>
        public static bool CleanInput(string input, out string cleaned)
        {
            cleaned = "";

            if (string.IsNullOrWhiteSpace(input))
                return false;

            cleaned = CleanNumberPattern().Replace(input, "");

            cleaned = DublicatePattern().Replace(cleaned, ",");

            cleaned = cleaned.Trim(',');

            if (string.IsNullOrEmpty(cleaned))
                return false;

            return true;
        }

        [GeneratedRegex(@"[^0-9,\-]")]
        private static partial Regex CleanNumberPattern();
        [GeneratedRegex(@",{2,}")]
        private static partial Regex DublicatePattern();
    }
}
