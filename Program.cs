using System;

namespace traffic_light_simulation
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new World())
                game.Run();
        }
    }
}
