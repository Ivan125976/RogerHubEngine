namespace Yocto_Roger.Yocto_Roger.UtilityTools
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
    public class RogerMath(Parameters param)
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
        /// <returns></returns>
        public static double Tanh(double value)
        {
            return Math.Tanh(value);
        }
    }
}
