using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.UI;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.debug
{
    public class ReplayManager
    {
        private static ReplayManager _instance;
        private static readonly object Padlock = new object();
        private ReplayManager() {}
        public static ReplayManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new ReplayManager();
                    }
                    return _instance;
                }
            }
        }
        
        private List<DebugLogServerData>_serverData;
        private List<DebugLogEntitySpawn> _entitySpawn;
        
        private bool _sentServerDataWarning = false;
        private bool _sentEntitySpawnWarning = false;
        private int _currentEntity = 0;
        private int _currentServerRequest = 0;
            
        public bool ProcessLogs()
        {
            try
            {
                string serverJson = File.ReadAllText(Path.Combine(Logger.Instance.LoggingPath, "LatestServerLog.Json"));
                string entityJson = File.ReadAllText(Path.Combine(Logger.Instance.LoggingPath, "LatestEntityLog.Json"));
                
                _serverData = JsonSerializer.Deserialize<List<DebugLogServerData>>(serverJson);
                _entitySpawn = JsonSerializer.Deserialize<List<DebugLogEntitySpawn>>(entityJson);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public void CheckTick()
        {
            if (_currentEntity < _entitySpawn.Count)
            {
                while (_entitySpawn[_currentEntity].Tick == DebugManager.Instance.UpdateTick)
                {
                    switch (_entitySpawn[_currentEntity].EntityType)
                    {
                        case "Car":
                            Car car = Car.CreateReplayInstance(_entitySpawn[_currentEntity].DirectionMap);
                            VehicleEm.Instance.Subscribe(car);
                            break;
                    
                        case "Bike":
                            Bike bike = Bike.CreateReplayInstance(_entitySpawn[_currentEntity].DirectionMap);
                            VehicleEm.Instance.Subscribe(bike);
                            break;
                    
                        case "Person":
                            People guy = People.CreateReplayInstance(_entitySpawn[_currentEntity].DirectionMap);
                            VehicleEm.Instance.Subscribe(guy);
                            break;
                    
                        case "Boat":
                            break;
                    
                        default:
                            break;
                    }

                    _currentEntity += 1;
                    if (_currentEntity == _entitySpawn.Count)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (!_sentEntitySpawnWarning)
                {
                    _sentEntitySpawnWarning = true;
                    UiHandler.Instance.AddWarning("No more entities left in this replay");
                }
            }

            if (_currentServerRequest < _serverData.Count)
            {
                while (_serverData[_currentServerRequest].Tick == DebugManager.Instance.UpdateTick)
                {
                    States state;
                    switch (_serverData[_currentServerRequest].data.state)
                    {
                        case "GREEN":
                            state = States.Green;
                            break;
                        case "ORANGE":
                            state = States.Orange;
                            break;
                        case "BLINKING":
                            state = States.Orange;
                            break;
                        case "RED":
                            state = States.Red;
                            break;
                        default:
                            state = States.Red;
                            break;
                    }
                    
                    switch (_serverData[_currentServerRequest].eventType)
                    {
                        case "SET_AUTOMOBILE_ROUTE_STATE":
                            TrafficLightEm.Instance.OnStateChange(_serverData[_currentServerRequest].data.routeId, state);
                            Console.WriteLine("SET_AUTOMOBILE_ROUTE_STATE");
                            break;
                        case "SET_CYCLIST_ROUTE_STATE":
                            BicycleLightEm.Instance.OnStateChange(_serverData[_currentServerRequest].data.routeId, state);
                            Console.WriteLine("SET_CYCLIST_ROUTE_STATE");
                            break;
                        case "SET_PEDESTRIAN_ROUTE_STATE":
                            PedestrianLightEm.Instance.OnStateChange(_serverData[_currentServerRequest].data.routeId, state);
                            Console.WriteLine("SET_PEDESTRIAN_ROUTE_STATE");
                            break;
                    
                        default:
                            break;
                    }

                    _currentServerRequest += 1;
                    if (_currentServerRequest == _serverData.Count)
                    {
                        break;
                    }
                }
            }
            else
            {
                if (!_sentServerDataWarning)
                {
                    _sentServerDataWarning = true;
                    UiHandler.Instance.AddWarning("No more server Requests left in this replay");
                }
            }
        }
    }
}