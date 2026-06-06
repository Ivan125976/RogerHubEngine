namespace Yocto_Roger.IO
{
    internal class Splitter
    {
        /// <summary>
        /// Splitting a string into parts using a symbol
        /// </summary>
        /// <param name="obj">The string to be splitted</param>
        /// <param name="symbol">The character by which the string will be splitted</param>
        /// <returns></returns>
        public static int[] StringParse(string obj, char symbol)
        {
            string[] strings = obj.Split(symbol);
            int[] parsedArray = new int[strings.Length];
            for (int i = 0; i < parsedArray.Length; i++)
                parsedArray[i] = Convert.ToInt32(strings[i]);
            return parsedArray;
        }
    }
}
