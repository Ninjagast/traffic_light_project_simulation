using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.dataClasses.ServerRequests
{
    public class ServerEntityEnteredZoneRequest
    {
        public string eventType { get; set; } = "ENTITY_ENTERED_ZONE";
        public RouteSensorData data { get; set; }
    }
}