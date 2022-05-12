using System.Collections.Generic;

namespace traffic_light_simulation.classes.dataClasses.ServerRequests
{
    public class AcknowledgeDataRequest
    {
        public string eventType { get; set; }
        public states data { get; set; }
    }

    public class states
    {
        public string state { get; set; }
    }
}