using RogerHubEngine.Engine.RRNNs;
using RogerHubEngine.Engine.UI;

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

            ConsoleManager.NewLine();

            StringList buildType = new()
            {
                borderStyle = StringList.BorderStyles.StarsCorners
            };

            buildType.Add("P > Physical Build");
            buildType.Add("V > Virtual Build");
            buildType.Add("Q > Quit");
            buildType.Draw();

            ConsoleManager.NewLine();

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
