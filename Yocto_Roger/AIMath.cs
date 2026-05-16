namespace Yocto_Roger
{
    internal class AIMath
    {
        static public Random rand = new();
        public static void Rounding(ref double[] neurons)
        {
            for (int i = 0; i < neurons.Length; i++)
                neurons[i] = (int)Math.Round(neurons[i]);
        }
        public static double Sigmoida(double value) //активация
        {
            double answer = 1.0 / (1.0 + Math.Exp(-value));
            if (Parameters.isDebug)
                Console.WriteLine("Sigmoida> " + answer);
            return answer;
        }
        public static int BinToNum(double[] binary) //двиучное в десятичное
        {
            int result = 0;
            for (int i = 0; i < binary.Length; i++)
                if (binary[i] != 0.0)
                    result += 1 << (7 - i);
            if (Parameters.isDebug)
                Console.WriteLine("binToNum -> " + result);
            return result;
        }
        public static int[] NumToBin(int num, int bits) //конвертация десятичного числа в двиучное
        {
            if(Parameters.isDebug)
                Console.WriteLine("numToBin (" + bits + " bits) -> ");
            int[] bin = new int[bits];
            for (int i = 0; i < bits; i++)
            {
                bin[bits - 1 - i] = (num >> i) & 1;
                if (Parameters.isDebug)
                    Console.Write(bin[bits - 1 - i] + " ");
            }
            return bin;
        }
    }
}
