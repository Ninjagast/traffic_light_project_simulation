using System.Collections.Generic;
using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.dataClasses
{
    public class ExtensionsMap
    {
        public string key { get; set; }
        public List<List<Directions>> directions { get; set; }
    }
}