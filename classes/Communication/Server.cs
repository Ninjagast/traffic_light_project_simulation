using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using WebSocketSharp;
using System.Text.Json;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using WebSocket = WebSocketSharp.WebSocket;
using Logger = traffic_light_simulation.classes.debug.Logger;


namespace traffic_light_simulation.classes.Communication
{
    public class Server
    {
        private static Server _instance;
        private static readonly object Padlock = new object();

        private Server() {}
        
        public static Server Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new Server();
                    }
                    return _instance;
                }
            }
        }
        
        private readonly string _address = "ws://keyslam.com:8080";
        private WebSocket _webSocket;
        public bool HasConnection = false; 
        private string _sessionName;
        private string _sessionVersion;

        public void StartServer()
        {
            _webSocket = new WebSocket(_address);
            _webSocket.OnOpen += (sender, e) =>
            {
                connectData connectData = new connectData
                {
                    sessionName = _sessionName,
                    sessionVersion = int.Parse(_sessionVersion),
                    discardParseErrors = false,
                    discardEventTypeErrors = false,
                    discardMalformedDataErrors = false,
                    discardInvalidStateErrors = false
                };
                
                ServerConnectRequest connectRequest = new ServerConnectRequest {data = connectData};

                _webSocket.Send (JsonSerializer.Serialize(connectRequest));
            };
            
            _webSocket.OnMessage += _onMessage;
                
            _webSocket.OnClose += (sender, e) =>
            {
                Console.WriteLine ($"closed; Reason was: {e.Reason}");
            };
            
            _webSocket.OnError += (sander, e) =>
            {
                Console.WriteLine("error pik");
                Console.WriteLine(e.Message);
            };
            
            _webSocket.Connect();
        }

        private void _onMessage(object sender, MessageEventArgs e)
        {
            ServerData data = JsonSerializer.Deserialize<ServerData>(e.Data);

            if (data != null)
            {
                if (DebugManager.Instance.Logging)
                {
                    DebugServerData log = new DebugServerData
                    {
                        data = data.data,
                        eventType = data.eventType,
                        Tick = DebugManager.Instance.UpdateTick
                    };
                    Logger.Instance.LogServerMessage(log);
                }
                
                if (data.eventType == "SESSION_START" || data.eventType == "SESSION_STOP")
                {
                    HasConnection = data.eventType == "SESSION_START";
                    Console.WriteLine($"{data.eventType}]");
                    return;
                }

                States state;
                switch (data.data.state)
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
                        Console.WriteLine($"Unknown state type {data.data.state}");
                        break;
                }

                switch (data.eventType)
                {
                    case "SET_AUTOMOBILE_ROUTE_STATE":
                        TrafficLightEm.Instance.OnStateChange(data.data.routeId, state);
                        Console.WriteLine("SET_AUTOMOBILE_ROUTE_STATE");
                        break;
                    case "SET_CYCLIST_ROUTE_STATE":
                        BicycleLightEm.Instance.OnStateChange(data.data.routeId, state);
                        Console.WriteLine("SET_CYCLIST_ROUTE_STATE");
                        break;
                    case "SET_PEDESTRIAN_ROUTE_STATE":
                        PedestrianLightEm.Instance.OnStateChange(data.data.routeId, state);
                        Console.WriteLine("SET_PEDESTRIAN_ROUTE_STATE");
                        break;

                    default:
                        Console.WriteLine($"unknown eventType {data.eventType}");
                        break;
                }
            }
        }

        public void EntityEnteredZone(int routeId)
        {
            Console.WriteLine("EntityEnteredZone");
            RouteSensorData data = new RouteSensorData {routeId = routeId, sensorId = 1};
            ServerEntityEnteredZoneRequest serverRequest = new ServerEntityEnteredZoneRequest {data = data};
            _webSocket.Send (JsonSerializer.Serialize(serverRequest));
        }

        public void EntityExitedZone(int routeId)
        {
            Console.WriteLine("EntityExitedZone");
            RouteSensorData data = new RouteSensorData {routeId = routeId, sensorId = 1};
            ServerEntityEnteredZoneRequest serverRequest = new ServerEntityEnteredZoneRequest {data = data, eventType = "ENTITY_EXITED_ZONE"};
            _webSocket.Send (JsonSerializer.Serialize(serverRequest));
        }

        public void SetServerNameVersion(string name, string version)
        {
            _sessionName = name;
            _sessionVersion = version;
        }
    }
}

