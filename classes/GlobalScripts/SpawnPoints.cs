using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Xna.Framework;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.EventManagers;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class SpawnPoints
    {
        private List<DirectionMap> _landSpawnPoints;
        private List<DirectionMap> _waterSpawnPoints;
        private Random _random = new Random();
        
        private static SpawnPoints _instance;
        private static readonly object Padlock = new object();

        private SpawnPoints() {}
        
        public static SpawnPoints Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new SpawnPoints();
                    }
                    return _instance;
                    _instance.GetSpawnPoints();
                }
            }
        }

        public void GetSpawnPoints()
        {
            string path = "../../../LandRoutes.json";
            if (File.Exists(path))
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    _landSpawnPoints = JsonSerializer.Deserialize<List<DirectionMap>>(json);
                }
            }
        }

        public DirectionMap GetRandomLandSpawnPoint()
        {
            return _landSpawnPoints[(_random.Next(_landSpawnPoints.Count))];
        }

        public DirectionMap GetRandomWaterSpawnPoint()
        {
            return _waterSpawnPoints[(_random.Next(_waterSpawnPoints.Count))];
        }
    }
}