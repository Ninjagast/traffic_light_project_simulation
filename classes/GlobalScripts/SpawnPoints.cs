using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class SpawnPoints
    {
        private List<Vector2> _landSpawnPoints;
        private List<Vector2> _waterSpawnPoints;
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
                }
            }
        }

//      todo finish this function when the waypoint system is done
        public void GetSpawnPoints()
        {
            
        }

        public Vector2 GetRandomLandSpawnPoint()
        {
            return _landSpawnPoints[_random.Next(_landSpawnPoints.Count)];
        }

        public List<Vector2> GetLandSpawnPoints()
        {
            return _landSpawnPoints;
        }

        public Vector2 GetRandomWaterSpawnPoint()
        {
            return _waterSpawnPoints[_random.Next(_waterSpawnPoints.Count)];
        }

        public List<Vector2> GetWaterSpawnPoints()
        {
            return _waterSpawnPoints;
        }
    }
}