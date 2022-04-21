using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            //Left crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(380,520), 9, new Vector2(375, 520)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(430,550), 8, new Vector2(425, 545)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(480,570), 8, new Vector2(475, 570)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(530,590), 7, new Vector2(525, 595)));
            
            //Bottom crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1400,500), 4, new Vector2(1380, 520)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1350,525), 4, new Vector2(1320, 540)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1300,550), 5, new Vector2(1275, 570)));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(590,700), 15, ont));
            
            //Right crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(800,101), 1, new Vector2(775, 115)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(850,130), 2, new Vector2(825, 140)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(900,160), 2, new Vector2(875, 165)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(950,180), 3, new Vector2(925, 190)));
            
            //Top crosspoint
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
            UiHandler.Instance.Subscribe(new ButtonBase(400, 200, "PlayButton", ButtonStates.PlayButton, TextureManager.Instance.GetButtonTexture("PlayButton")));
            UiHandler.Instance.Subscribe(new ButtonBase(300, 200, "DebugButton", ButtonStates.DebugButton, TextureManager.Instance.GetButtonTexture("DebugButton")));
            UiHandler.Instance.Subscribe(new ButtonBase(300, 100, "SessionNameField", ButtonStates.SessionNameField, TextureManager.Instance.GetButtonTexture("FieldTexture")));
            UiHandler.Instance.Subscribe(new ButtonBase(300, 150, "SessionVersionField", ButtonStates.SessionVersionField, TextureManager.Instance.GetButtonTexture("FieldTexture")));
        }

        public static void CreateDebugButtons()
        {
            UiHandler.Instance.Subscribe(new ButtonBase(400, 200, "DebugPlayButton", ButtonStates.DebugPlayButton, TextureManager.Instance.GetButtonTexture("PlayButton")));
            UiHandler.Instance.Subscribe(new ButtonBase(300, 200, "ShowClaimedCellsRadio", ButtonStates.ShowClaimedCellsRadio, TextureManager.Instance.GetButtonTexture("DebugButton")));
        }
    }
}