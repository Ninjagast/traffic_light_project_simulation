using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.dataClasses.ServerRequests
{
    public class ServerConnectRequest
    {
        public string eventType { get; set; } = "CONNECT_SIMULATOR";
        public ConnectData data { get; set; }
    }
}