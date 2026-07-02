using RogerHubEngine.Engine.Processes;
using System.Diagnostics;

namespace RogerHubEngine.Engine.Graphics
{
    internal class Render : IProcess
    {
        public char[,] bufferData = new char[Console.WindowWidth, Console.WindowHeight];

        public ParameterizedThreadStart StartProcess(int fps)
        {
            int frame = 1000 / fps;
            Stopwatch sw = new();
            while (true)
            {
                sw.Restart();
                for (int i = 0; i < bufferData.GetLength(0); i++)
                {
                    for (int j = 0; j < bufferData.GetLength(1); j++)
                    {
                        Console.Write(bufferData[i, j]);
                    }
                    Console.WriteLine();
                }    
                sw.Stop();
                int delay = frame - (int)sw.ElapsedMilliseconds;
                if (delay > 0)
                    Thread.Sleep(delay);
            }
        }
    }
}
