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
        private readonly string _address = "ws://keyslam.com:8080";
        private WebSocket _webSocket;
        
        public List<MessageEventArgs> requestHistory = new List<MessageEventArgs>(); 
        
        public void StartServer()
        {
            _webSocket = new WebSocket(_address);
            _webSocket.OnOpen += (sender, e) =>
            {
                String json =
                    "{\"eventType\" : \"CONNECT_SIMULATOR\",  " +
                    "\"data\" : " +
                    "{ \"sessionName\" : \"KFC\", " +
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
                Console.WriteLine($"{data.eventType}]");
                return;
            }
            

            States state;
            switch (data.data.state)
            {
                case "GREEN":
                    state = States.GREEN;
                    break;
                case "ORANGE":
                    state = States.ORANGE;
                    break;
                case "BLINKING":
                    state = States.ORANGE;
                    break;
                case "RED":
                    state = States.RED;
                    break;
                default:
                    state = States.RED;
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
}

