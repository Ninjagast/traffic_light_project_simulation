using System;
using System.Collections.Generic;
using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class RouteTable
    {
        private static RouteTable _instance;
        private static readonly object Padlock = new object();
        private RouteTable() {}
        public static RouteTable Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new RouteTable();
                    }
                    return _instance;
                }
            }
        }

        private Dictionary<string, int> _routeTable = new Dictionary<string, int>();
        private int _tableSum = 0;
        private Random _random;

        public void CreateTable(Random random)
        {
            _random = random;
            AddToRouteTable(2, "Route 12");
            AddToRouteTable(2, "Route 11");
            AddToRouteTable(2, "Route 10");
            AddToRouteTable(2, "Route 9");
            AddToRouteTable(2, "Route top 8");
            AddToRouteTable(2, "Route bot 8");
            AddToRouteTable(2, "Route 7");
            AddToRouteTable(1, "Route 5 Going left");
            AddToRouteTable(1, "Route 5 Going top");
            AddToRouteTable(2, "Route Left 4");
            AddToRouteTable(2, "Route Right 4");
            AddToRouteTable(2, "Route 15");
            AddToRouteTable(2, "right roundabout");
            AddToRouteTable(1, "bottom roundabout first right");
            AddToRouteTable(1, "bottom roundabout full send");
        }

        public void AddToRouteTable(int weight, string name)
        {
            _routeTable.Add(name, weight);
            _tableSum += weight;
        }
        
        public DirectionMap GetRandomRoute()
        {
            int neo = _random.Next(0, _tableSum); // the one
            int key = 0;
            foreach (var route in _routeTable)
            {
                neo -= route.Value;

                if (neo < 0)
                {
                    return SpawnPoints.Instance.GetRandomLandSpawnPoint(key);
                }
                else
                {
                    key++;
                }
            }
//          If it ever gets here.
//          Then You better get dressed for your ceremony.
//          Since you just broke the laws of mathematics!!! 
//          Have fun with your field medal!
            return SpawnPoints.Instance.GetRandomLandSpawnPoint(0);
        }
    }
}