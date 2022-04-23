using System;
using traffic_light_simulation.classes.debug;

namespace traffic_light_simulation
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
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
