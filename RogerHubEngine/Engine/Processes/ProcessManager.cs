namespace RogerHubEngine.Engine.Processes
{
    public interface IProcess
    {
        public ParameterizedThreadStart StartProcess(int TargetFPS);
    }

    public class Process(Thread thread, ProcessType type, string name = "New Thread")
    {
        protected readonly Thread? _thread = thread;
        public string Name { get; private set; } = name;

        public ProcessType Type { get; private set; } = type;
        public void Start() => _thread?.Start();
    }

    public enum ProcessType
    {
        nn_helper,
        drawer,
        render,
        IO,
        network,
        other
    }
}
