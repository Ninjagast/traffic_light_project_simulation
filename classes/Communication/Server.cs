using System;
using WebSocketSharp;
using System.Text.Json;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.dataClasses.ServerRequestData;
using traffic_light_simulation.classes.dataClasses.ServerRequests;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using Logger = traffic_light_simulation.classes.debug.Logger;
using System.Threading;


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
        private string _sessionName;
        private string _sessionVersion;
        public bool HasConnection; 
        private WebSocket _webSocket = null;

        public void StartServer()
        {
            _webSocket = new WebSocket(_address);
//          setting up websocket functions
            _webSocket.OnOpen += (sender, e) =>
            {
                ConnectData connectData = new ConnectData
                {
                    sessionName                = _sessionName,
                    sessionVersion             = int.Parse(_sessionVersion),
                    discardParseErrors         = false,
                    discardEventTypeErrors     = false,
                    discardMalformedDataErrors = false,
                    discardInvalidStateErrors  = false
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
                Console.WriteLine("Server error");
                Console.WriteLine(e.Message);
            };
            
            _webSocket.Connect();
        }

        private void _onMessage(object sender, MessageEventArgs e)
        {
            ServerData data = JsonSerializer.Deserialize<ServerData>(e.Data);

            if (data != null)
            {
                if (data.eventType == "SESSION_START" || data.eventType == "SESSION_STOP")
                {
                    HasConnection = data.eventType == "SESSION_START";
                    Console.WriteLine($"{data.eventType}]");
                    return;
                }
                
                if (DebugManager.Instance.Logging)
                {
                    DebugLogServerData log = new DebugLogServerData
                    {
                        data = data.data,
                        eventType = data.eventType,
                        Tick = DebugManager.Instance.UpdateTick
                    };
                    Logger.Instance.LogServerMessage(log);
                }

                if (data.eventType == "REQUEST_BRIDGE_ROAD_EMPTY")
                {
                    Thread.Sleep(4000);
                    AcknowledgeBridgeEmpty();
                    return;
                }

                if (data.eventType == "REQUEST_BARRIERS_STATE")
                {
                    if (data.data.state == "DOWN")
                    {
                        BridgeHitTreeEm.Instance.OnStateChange(0, States.Closed);
                        AcknowledgeHitTreeState(false);
                    }
                    else
                    {
                        BridgeHitTreeEm.Instance.OnStateChange(0, States.Open);
                        AcknowledgeHitTreeState(true);
                    }
                    return;
                }

                if (data.eventType == "REQUEST_BRIDGE_STATE")
                {
                    if (data.data.state == "DOWN")
                    {
                        BridgeEm.Instance.OnStateChange(0, States.Closed);
                        AcknowledgeBridgeState(false);
                    }
                    else
                    {
                        BridgeEm.Instance.OnStateChange(0, States.Open);
                        AcknowledgeBridgeState(true);
                    }                    
                    return;
                }

                if (data.eventType == "REQUEST_BRIDGE_WATER_EMPTY")
                {
                    Thread.Sleep(10000);
                    AcknowledgeWaterEmpty();
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
                    case "GREENRED":
                        state = States.Orange;
                        break;
                    case "ON":
                        state = States.Red;
                        break;
                    case "OFF":
                        state = States.Green;
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
                    case "SET_BRIDGE_WARNING_LIGHT_STATE":
                        BridgeWarningLightEm.Instance.OnStateChange(0, state);
                        Console.WriteLine("SET_BRIDGE_WARNING_LIGHT_STATE");
                        break;                    
                    case "SET_BOAT_ROUTE_STATE":
                        TrafficLightEm.Instance.OnStateChange(data.data.routeId, state);
                        Console.WriteLine("SET_BOAT_ROUTE_STATE");
                        break;
                    default:
                        Console.WriteLine($"unknown eventType {data.eventType}");
                        break;
                }
            }
        }

        public void EntityEnteredZone(int routeId)
        {
            if (EventManagerEm.Instance.State != SimulationStates.Replaying)
            {
                RouteSensorData data = new RouteSensorData {routeId = routeId, sensorId = 1};
                ServerEntityEnteredZoneRequest serverRequest = new ServerEntityEnteredZoneRequest {data = data};
                _webSocket.Send (JsonSerializer.Serialize(serverRequest));
            }
        }

        public void EntityExitedZone(int routeId)
        {
            if (EventManagerEm.Instance.State != SimulationStates.Replaying)
            {
                RouteSensorData data = new RouteSensorData {routeId = routeId, sensorId = 1};
                ServerEntityEnteredZoneRequest serverRequest = new ServerEntityEnteredZoneRequest {data = data, eventType = "ENTITY_EXITED_ZONE"};
                _webSocket.Send (JsonSerializer.Serialize(serverRequest));
            }
        }
   
        public void AcknowledgeBridgeEmpty()
        {
            if (EventManagerEm.Instance.State != SimulationStates.Replaying)
            {
                AcknowledgeRequest data = new AcknowledgeRequest {eventType = "ACKNOWLEDGE_BRIDGE_ROAD_EMPTY"};
                _webSocket.Send (JsonSerializer.Serialize(data));
            }
        }        
        
        public void AcknowledgeWaterEmpty()
        {
            if (EventManagerEm.Instance.State != SimulationStates.Replaying)
            {
                AcknowledgeRequest data = new AcknowledgeRequest {eventType = "ACKNOWLEDGE_BRIDGE_WATER_EMPTY"};
                _webSocket.Send (JsonSerializer.Serialize(data));
            }
        }

        public void AcknowledgeHitTreeState(bool state)
        {
            if (EventManagerEm.Instance.State != SimulationStates.Replaying)
            {
                AcknowledgeDataRequest data = new AcknowledgeDataRequest {eventType = "ACKNOWLEDGE_BARRIERS_STATE", data = new states{state = !state ? "DOWN" : "UP"}};
                _webSocket.Send (JsonSerializer.Serialize(data));
            }
        }

        public void AcknowledgeBridgeState(bool state)
        {   
            if (EventManagerEm.Instance.State != SimulationStates.Replaying)
            {
                AcknowledgeDataRequest data = new AcknowledgeDataRequest {eventType = "ACKNOWLEDGE_BRIDGE_STATE", data = new states{state = !state ? "DOWN" : "UP"}};
                _webSocket.Send (JsonSerializer.Serialize(data));
            }
        }

        public void SetServerNameVersion(string name, string version)
        {
            _sessionName = name;
            _sessionVersion = version;
        }
    }
}

