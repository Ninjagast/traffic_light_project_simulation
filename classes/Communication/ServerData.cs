using traffic_light_simulation.classes.dataClasses;

namespace traffic_light_simulation.classes.Communication
{
    public class ServerData
    {
        public string eventType { get; set; }
        public dataObject data { get; set; }
    }
}