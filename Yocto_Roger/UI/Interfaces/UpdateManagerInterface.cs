using Velopack;
using Velopack.Sources;
using Yocto_Roger.UI.CUI;
using static Yocto_Roger.UI.CUI.CUI;

namespace Yocto_Roger.UI.Interfaces
{
    /// <summary>
    /// Class of Update Manager
    /// </summary>
    public class UpdateManagerInterface : IUserInterface
    {
        /// <summary>
        /// Menu of update manager + logic and exceptions. Can writing some text in console, be careful when using it in your project
        /// </summary>
        public void StartInterface()
        {
            VelopackApp.Build().Run();

            GithubSource githubSource = new("https://github.com/Ivan125976/RogerHubEngine", null, false);
            var mgr = new UpdateManager(githubSource);

            if (mgr.IsInstalled)
            {
                try
                {
                    UpdateInfo? info = mgr.CheckForUpdates();

                    if (info != null)
                    {
                        Console.WriteLine($" Update found! New version: {info.TargetFullRelease}. Do you want to install it? (y/n)\n >>>");
                        switch (Console.ReadKey().KeyChar)
                        {
                            case 'Y' or 'y':
                                {
                                    try
                                    {
                                        mgr.DownloadUpdates(info);
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"Failed to download the update: {ex}");
                                    }
                                    Console.WriteLine("Updates was downloaded successful!\nTrying to apply it, the app will be restarted in new version...");
                                    Thread.Sleep(5000); // For user can to read the message
                                    try
                                    {
                                        mgr.ApplyUpdatesAndRestart(info);
                                    }
                                    catch (Exception ex)
                                    {
                                        Send($"Failed to apply updates, here's my error: {ex}", MessageType.error);
                                    }
                                    break;
                                }
                        }
                    }
                    else
                    {
                        Send("Hey, you have the latest version");
                        Console.WriteLine("Press any ker to continue");
                        Console.ReadKey(true);
                    }
                }
                catch (Exception ex)
                {
                    Send($"{ex}", MessageType.error);
                }
            }
            else
                Send("You can't update in development mode", MessageType.error);
        }
    }
}


