using RogerHubEngine.Engine.Processes;

namespace RogerHubEngine.Engine.Graphics
{
    internal class TUI
    {
        static readonly Render render = new();
        public static void SrartTUI(int fps)
        {
            Thread renderThread = new(render.StartProcess(fps));
            Process renderProcess = new(renderThread, ProcessType.render, "Render");

            renderProcess.Start();
        }

        public static void SetPixel(int x, int y, char symbol) => render.bufferData[x, y] = symbol;
    }
}
