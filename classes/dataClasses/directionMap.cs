using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace traffic_light_simulation.classes.dataClasses
{
    public class DirectionMap
    {
        public V2 vector2{ get; set;}
        public List<Directions> directions { get; set; }
    }

    public class Directions
    {
        public int repeat { get; set; }
        public string direction { get; set; }
    }

    public class V2
    {
        public int x { get; set; }
        public int y { get; set; }
    }
}