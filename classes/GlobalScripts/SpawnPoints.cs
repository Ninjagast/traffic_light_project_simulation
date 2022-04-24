using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
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

        public DirectionMap GetRandomLandSpawnPoint(int key)
        {
            return _landSpawnPoints[key];
        }

        public DirectionMap GetRandomWaterSpawnPoint()
        {
            throw new NotImplementedException();
        }
    }
}