using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.dataClasses
{
    public class DebugLogEntitySpawn
    {
        public string EntityType { get; set; }
        public int Tick { get; set; }
        public DirectionMap DirectionMap { get; set; }
    }
}