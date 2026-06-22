using static Yocto_Roger.Configuration.EngineVersion;

namespace Yocto_Roger.UI.CUI
{
    internal class ASCIIDraw
    {
        public static void Logo(bool text)
        {
            if (text)
                Console.WriteLine($"""
                     _____                       _    _       _     ______             _            
                    |  __ \                     | |  | |     | |   |  ____|           (_)           
                    | |__) |___   __ _  ___ _ __| |__| |_   _| |__ | |__   _ __   __ _ _ _ __   ___ 
                    |  _  // _ \ / _` |/ _ \ '__|  __  | | | | '_ \|  __| | '_ \ / _` | | '_ \ / _ \
                    | | \ \ (_) | (_| |  __/ |  | |  | | |_| | |_) | |____| | | | (_| | | | | |  __/
                    |_|  \_\___/ \__, |\___|_|  |_|  |_|\__,_|_.__/|______|_| |_|\__, |_|_| |_|\___| V{majorVersion}.{minorVersion}
                                  __/ |                                           __/ |             
                                 |___/                                           |___/        
                    """);
            else
                Console.WriteLine("""                                
        ▒▒████████████▒▒        
      ▒▒                ▒▒      
      ██                ██      
      ██    ██    ██    ██      
      ██    ██    ██    ██      
      ██                ██      
      ██                ██      
    ▒▒██                ██▒▒    
  ▓▓  ██                ██  ▓▓  
  ██  ██                ██  ██  
  ██  ██                ██  ██  
  ▓▓  ██                ██  ▓▓  
    ▒▒██                ██▒▒    
      ██                ██      
      ██                ██             
""");
        }
    }
}
