using System;
using System.Collections.Generic;
using System.Runtime.Loader;
using WebSocketSharp;
using System.Text.Json;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using WebSocket = WebSocketSharp.WebSocket;


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
        
        public void StartServer()
        {
            _webSocket = new WebSocket(_address);
            _webSocket.OnOpen += (sender, e) =>
            {
                String json =
                    "{\"eventType\" : \"CONNECT_SIMULATOR\",  " +
                    "\"data\" : " +
                    "{ \"sessionName\" : \"burgerking\", " +
                    "\"sessionVersion\" : 1, " +
                    "\"discardParseErrors\" : false,  " +
                    "\"discardEventTypeErrors\" : false, " +
                    "\"discardMalformedDataErrors\" : false, " +
                    "\"discardInvalidStateErrors\" : false}" +
                    "}";
                _webSocket.Send (json);
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
            ServerData? data = JsonSerializer.Deserialize<ServerData>(e.Data);

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

        public void EntityEnteredZone(int routeId)
        {
            String json =
                "{\"eventType\" : \"ENTITY_ENTERED_ZONE\",  " +
                "\"data\" : " +
                "{ \"routeId\" : "+ routeId + ", " +
                "\"sensorId\" : 0}" +
                "}";
            
            _webSocket.Send (json);
        }

        public void EntityExitedZone(int laneId)
        {
            String json =
                "{\"eventType\" : \"ENTITY_EXITED_ZONE\",  " +
                "\"data\" : " +
                "{ \"routeId\" : "+ laneId + ", " +
                "\"sensorId\" : 0}" +
                "}";
            
            _webSocket.Send (json);
        }
    }
}

