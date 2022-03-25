using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Runtime.Loader;
using WebSocketSharp;
using WebSocket = WebSocketSharp.WebSocket;

namespace traffic_light_simulation
{
    public class Server
    {
        private WebSocket _webSocket;        
        public void StartServer()
        {
            _webSocket = new WebSocket("ws://keyslam.com:8080");
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
    }
}

