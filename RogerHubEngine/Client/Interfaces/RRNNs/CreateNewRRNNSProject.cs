using RogerHubEngine.Engine.RRNNs;

namespace RogerHubEngine.Client.Interfaces.RRNNs
{
    internal class CreateNewRRNNSProject
    {
        public static void StartInterface()
        {
            ConsoleManager.Clear();

            Build newBuild = new();

            Console.WriteLine("""

                RRNNs Compilator - New Project
                --------------------------------

                """);

            Console.WriteLine("Welcome to RRNN system Compilator!");
            Console.WriteLine("Step 1: Please select a build type for the new project.");
            Console.WriteLine("""
                *--------------------*
                : P > Physical Build :
                : V > Virtual Build  :
                : Q > Quit           :
                *--------------------*
                """);

            switch(Console.ReadKey(true).KeyChar.ToString().ToUpper())
            {
                case "P":
                    newBuild.bparams.Type = CompilationType.Physical;
                    break;

                case "V":
                    newBuild.bparams.Type = CompilationType.Virtual;
                    break;

                case "Q":
                    return;
            }
        }
    }
}
