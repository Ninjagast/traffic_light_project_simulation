namespace traffic_light_simulation.classes.dataClasses
{
    public class ServerConnectRequest
    {
        public string eventType { get; set; } = "CONNECT_SIMULATOR";
        public connectData data { get; set; }
    }
}