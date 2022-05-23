using System;
using traffic_light_simulation.classes.debug;
using System.Runtime.InteropServices;

namespace traffic_light_simulation
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
//          runs the program. Put it into a try catch in order to log even when the program crashes
            using (var game = new World())
                try
                {
                    game.Run();
                }
                catch (Exception e)
                {
                    Logger.Instance.CreateLogs();
                    throw;
                }
        }
    }
}
