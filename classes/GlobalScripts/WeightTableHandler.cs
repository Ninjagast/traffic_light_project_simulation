using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.dataClasses.ServerRequestData;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class WeightTableHandler
    {
        private static WeightTableHandler _instance;
        private static readonly object Padlock = new object();
        private WeightTableHandler() {}
        public static WeightTableHandler Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new WeightTableHandler();
                    }
                    return _instance;
                }
            }
        }

        private WeightTable _routeTable = new WeightTable();
        private Dictionary<string, WeightTable> _extensionTables = new Dictionary<string, WeightTable>();
        private int _tableSum = 0;
        private Random _random;

        public void CreateTables(Random random)
        {
            _random = random;

            _routeTable
                // .AddRow(3, "Route 12")
                // .AddRow(3, "Route 11")
                // .AddRow(3, "Route 10")
                // .AddRow(4, "Route 9")
                // .AddRow(5, "Route top 8")
                // .AddRow(5, "Route bot 8")
                // .AddRow(4, "Route 7")
                // .AddRow(3, "Route 5 Going left")
                // .AddRow(3, "Route 5 Going top")
                // .AddRow(5, "Route Left 4")
                // .AddRow(6, "Route Right 4")
                // .AddRow(1, "Route 15")
                // .AddRow(8, "right roundabout")
                // .AddRow(2, "bottom roundabout first right")
                .AddRow(3, "bottom roundabout full send");
//              max sum 60 for now

            WeightTable joinedRouteAfterRoundabout = new WeightTable();
            WeightTable afterCrossroad = new WeightTable();

            joinedRouteAfterRoundabout
                .AddRow(2, "Route 1")
                .AddRow(3, "Route top 2")
                .AddRow(3, "Route bot 2")
                .AddRow(2, "route 3");

            afterCrossroad
                .AddRow(1, "roundabout first right")
                .AddRow(2, "roundabout second right");

            
            _extensionTables.Add("1725445", joinedRouteAfterRoundabout);
            _extensionTables.Add("1525745", afterCrossroad);
        }

        public DirectionMap GetRandomRoute()
        {
            int neo = _random.Next(0, _routeTable.GetTableSum()); // the one
            int key = 0;
            foreach (var route in _routeTable)
            {
                neo -= route.Value;

                if (neo < 0)
                {
                    return SpawnPoints.Instance.GetLandSpawnPoint(key);
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
            return SpawnPoints.Instance.GetLandSpawnPoint(0);
        }

        public DirectionMap GetRandomExtension(Vector2 pos)
        {
            string posKey = $"{pos.X}{pos.Y}";
            if (posKey != "1725445" && posKey != "1525745")
            {
                return null;
            }
            
            int neo = _random.Next(0, _extensionTables[posKey].GetTableSum()); // the one
            int key = 0;
            
            foreach (var route in _extensionTables[posKey])
            {
                neo -= route.Value;

                if (neo < 0)
                {
                    List<Directions> directionsList =  SpawnPoints.Instance.GetExtension(posKey, key);
                    return new DirectionMap(){directions = directionsList};
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
            return null;
        }
    }
}