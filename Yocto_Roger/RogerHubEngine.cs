namespace Yocto_Roger
{

    internal class RogerHubEngine
    {
        const string version = "2.2.0";

        const char revision = ' ';

        static void Main()
        {
            UI.UI user = new();
            user.Start();
        }

        /// <summary>
        /// Allows you to get the application version
        /// </summary>
        /// <param name="withRevision">If enabled, the version will be returned as a string with a revision letter at the end.</param>
        /// <returns></returns>
        public static string GetVersion(bool withRevision)
        {
            if (!withRevision)
                return version;
            else
                return version + revision;
        }
    }
}
