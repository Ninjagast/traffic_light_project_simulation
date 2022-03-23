using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using WebSocketSharp;
using WebSocket = WebSocketSharp.WebSocket;

namespace traffic_light_simulation
{
    public class Server
    {
        // private String _adress;
        // private TcpClient _client;
        
        public async void StartServer()
        {
            String sessionName = "MacDonalds";
            int sessionsVersion = 1;
            String json =
                "{\"eventType\" : \"CONNECT_CONTROLLER\",  " +
                "\"data\" : " +
                    "{ \"sessionName\" : \"" + sessionName + "\", " +
                    "\"sessionVersion\" : 1, " +
                    "\"discardParseErrors\" : false,  " +
                    "\"discardEventTypeErrors\" : false, " +
                    "\"discardMalformedDataErrors\" : false, " +
                    "\"discardInvalidStateErrors\" : false}" +
                "}"; 

            
            using (var ws = new WebSocket ("ws://keyslam.com:8080")) {
                ws.OnMessage += (sender, e) =>
                    Console.WriteLine ("Laputa says: " + e.Data);

                ws.Connect ();
                ws.Send (json);
                Console.ReadKey (true);
            }
        }
    }
}

