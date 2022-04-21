using traffic_light_simulation.classes.enums;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class SimulationStateHandler
    {
        private static SimulationStateHandler _instance;
        private static readonly object Padlock = new object();

        private SimulationStateHandler() {}
        
        public static SimulationStateHandler Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new SimulationStateHandler();
                    }
                    return _instance;
                }
            }
        }
        
        public SimulationStates State = SimulationStates.StartScreen;
    }
}