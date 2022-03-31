using System;
using System.Collections.Generic;
using WebSocketSharp;
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
                    "{ \"sessionName\" : \"DubbleFF\", " +
                    "\"sessionVersion\" : 1, " +
                    "\"discardParseErrors\" : false,  " +
                    "\"discardEventTypeErrors\" : false, " +
                    "\"discardMalformedDataErrors\" : false, " +
                    "\"discardInvalidStateErrors\" : false}" +
                    "}";
                _webSocket.Send (json);
            };
            _webSocket.OnMessage += (sender, e) =>
            {
                Console.WriteLine("Broker says:");
                Console.WriteLine (e.Data);
                requestHistory.Add(e);
            };
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

        public void Send()
        {
            String json =
                "{\"eventType\" : \"ENTITY_ENTERED_ZONE\",  " +
                "\"data\" : " +
                    "{\"routeId\" : 1," +
                    "\"sensorId\" : 1}"+
                "}";
            _webSocket.Send (json);
        }
    }
}

