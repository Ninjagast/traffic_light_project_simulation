using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.dataClasses
{
    public class DebugLogServerData
    {
        public string eventType { get; set; }
        public RouteData data { get; set; }
        public int Tick {get; set; }
    }
}