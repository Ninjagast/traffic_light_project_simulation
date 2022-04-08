using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace traffic_light_simulation.classes.dataClasses
{
    public class directionMap
    {
        public V2 vector2{ get; set;}
        public Directions directions { get; set; }
    }

    public class Directions
    {
        public int repeat_first { get; set; }
        public List<string> directions { get; set; }
    }

    public class V2
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}