using System.Collections.Generic;
using Microsoft.Xna.Framework;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.UI;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.GlobalScripts
{
    public static class CreationManager
    {
        public static void CreateTrafficLights()
        {
//          Left of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(380,520), 9, new Vector2(375, 520)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(430,550), 8, new Vector2(425, 545)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(480,570), 8, new Vector2(475, 570)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(530,590), 7, new Vector2(525, 595)));
            
//          Bottom of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1400,500), 4, new Vector2(1380, 520)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1350,525), 4, new Vector2(1320, 540)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1300,550), 5, new Vector2(1275, 570)));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(590,700), 15, ont)); todo fix this route pls
            
            //Right of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(800,101), 1, new Vector2(775, 115)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(850,130), 2, new Vector2(825, 140)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(900,160), 2, new Vector2(875, 165)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(950,180), 3, new Vector2(925, 190)));
            
            //Top of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(290,250), 10,new Vector2(275, 270)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(340,230), 11,new Vector2(325, 245)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(390,210), 12,new Vector2(375, 220)));
        }

        public static void CreatePedestrianLights()
        {
            throw new System.NotImplementedException();
        }

        public static void CreateBicycleLights()
        {
            throw new System.NotImplementedException();
        }

        public static void CreateBoatLights()
        {
            throw new System.NotImplementedException();
        }

        public static void CreateStartScreenButtons()
        {
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(400, 200), ButtonStates.PlayButton, "PlayButton"));
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(300, 200), ButtonStates.DebugButton,"DebugButton"));
            
            UiHandler.Instance.Subscribe(new InputField(new Vector2(300, 100),"SessionNameField"));
            UiHandler.Instance.Subscribe(new InputField(new Vector2(300, 150),"SessionVersionField"));
        }

        public static void CreateDebugButtons()
        {
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,50),  "Show claimed cells",     DebugOptions.DrawClaimedCells,       new Vector2(-180,15),  "ClaimedCells"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,100), "Show CarIds",            DebugOptions.DrawCarIds,             new Vector2(-180,15),  "CarIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,150), "Show TrafficLightIds",   DebugOptions.DrawTrafficLightIds,    new Vector2(-180,15),  "TrafficLightIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,200), "Show BicycleLightIds",   DebugOptions.DrawBicycleLightIds,    new Vector2(-180,15),  "BicycleLightIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200, 250),"Show PedestrianLightIds",DebugOptions.DrawPedestrianLightIds, new Vector2(-180, 15), "PedestrianLightIds"));

            UiHandler.Instance.Subscribe(new RadioButtonGroup(new Vector2(650,50), "LoggingReplay", new Dictionary<string, DebugOptions> {{"logging", DebugOptions.Logging}, {"replay", DebugOptions.Replay}}, new List<string> {"Turn on the logger?", "Use latest replay?"} ,new Vector2(0, 75), new Vector2(-150, 15)));
            
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(350, 650), ButtonStates.DebugPlayButton, "PlayButton"));
        }
    }
}