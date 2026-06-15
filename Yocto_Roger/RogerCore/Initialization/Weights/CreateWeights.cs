namespace Yocto_Roger.RogerCore.Initialization.Weights
{
    /// <summary>
    /// Creates an array of middle weights
    /// </summary>
    public class CreateWeights
    {
        /// <summary>
        /// Creates an array of middle weights.
        /// </summary>
        /// <param name="weights">Array of middle weights</param>
        /// <param name="size">Size of array</param>
        public static void CreateMiddleWeights(double[][,] weights, int size)
        {
            for (int i = 0; i < weights.Length; i++)
                weights[i] = new double[size, size];
        }
    }
}
