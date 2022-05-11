using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.dataClasses.ServerRequestData;


namespace traffic_light_simulation.classes.GlobalScripts
{
    public class SpawnPoints
    {
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
                }
            }
        }
        
        private List<DirectionMap> _landSpawnPoints;
        private List<DirectionMap> _waterSpawnPoints;
        private List<DirectionMap> _sideWalkRoutes;
        private List<ExtensionsMap> _mapExtensions;

        public void GetSpawnPoints()
        {
            try
            {
                using (StreamReader r = new StreamReader("../../../LandRoutes.json"))
                {
                    string json = r.ReadToEnd();
                    _landSpawnPoints = JsonSerializer.Deserialize<List<DirectionMap>>(json);
                }
                using (StreamReader r = new StreamReader("../../../RouteExtentions.json"))
                {
                    string json = r.ReadToEnd();
                    _mapExtensions = JsonSerializer.Deserialize<List<ExtensionsMap>>(json);
                }                
                
                using (StreamReader r = new StreamReader("../../../SideWalkRoutes.json"))
                {
                    string json = r.ReadToEnd();
                    _sideWalkRoutes = JsonSerializer.Deserialize<List<DirectionMap>>(json);
                }                
                
                using (StreamReader r = new StreamReader("../../../SeaRoutes.json"))
                {
                    string json = r.ReadToEnd();
                    _waterSpawnPoints = JsonSerializer.Deserialize<List<DirectionMap>>(json);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Route files did not exist");
                throw;
            }

        }

        public DirectionMap GetLandSpawnPoint(int key)
        {
            return _landSpawnPoints[key];
        }

        public List<Directions> GetExtension(string positionId, int key)
        {
            foreach (var extensionsMap in _mapExtensions)
            {
                if (extensionsMap.key == positionId)
                {
                    return extensionsMap.directions[key];
                }
            }

            throw new Exception($"Extension with key {positionId} does not exist");
        }
        
        public DirectionMap GetSideWalkSpawn(int key)
        {
            return _sideWalkRoutes[key];
        }

        public DirectionMap GetSeaRoute(int key)
        {
            return _waterSpawnPoints[key];
        }
    }
}