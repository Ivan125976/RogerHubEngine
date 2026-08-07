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
    public partial class RogerMath()
    {
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

        /// <summary>
        /// Generates a random value with a Gaussian (normal) distribution.
        /// </summary>
        /// <param name="shift">Shift</param>
        /// <param name="stdDev">Standart Deviation</param>
        public static double NextGaussian(double shift, double stdDev)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                                   Math.Sin(2.0 * Math.PI * u2);

            return shift + stdDev * randStdNormal;
        }
    }
}
