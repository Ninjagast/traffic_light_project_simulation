using Microsoft.Xna.Framework;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.UI;
using traffic_light_simulation.classes.UI.buttons;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.GlobalScripts
{
    public static class CreationManager
    {
        public static void CreateTrafficLights()
        {
//          Left of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(425,1195), 9, new Vector2(425, 1195)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(475,1220),8, new Vector2(475,1220)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(525,1245),8, new Vector2(525, 1245)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(575,1270), 7, new Vector2(575, 1270)));
            
//          Bottom of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1275, 1170), 4, new Vector2(1275, 1170)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1225,1195), 4, new Vector2(1225,1195)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1175,1220), 5, new Vector2(1175,1220)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1275,1120), 15, new Vector2(1275,1120)));
            
//          Right of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(825,795), 1, new Vector2(825, 795)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(875,820), 2, new Vector2(875, 820)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(925,845), 2, new Vector2(925, 845)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(975,870), 3, new Vector2(975, 870)));
            
//          Top of the intersection
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(275,970), 10,new Vector2(275, 970)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(325,945), 11,new Vector2(325, 945)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(375,920), 12,new Vector2(375, 920)));
        }

        public static void CreatePedestrianLights()
        {
//          Bottom of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(815, 1185), 34, new Vector2(825, 1195)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(925, 1145), 34, new Vector2(925, 1145)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(915, 1130), 33, new Vector2(925, 1145)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(1125, 1045),33, new Vector2(1125, 1045)));
            
//          Left of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(700, 1260), 35, new Vector2(675, 1270)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(425, 1145), 35, new Vector2(425, 1145)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(435, 1135), 36, new Vector2(425, 1145)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(235, 1040), 36, new Vector2(225, 1045)));
            
//          Right of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(925, 945),  32, new Vector2(925, 945))); 
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(1085, 1010),32, new Vector2(1075, 1020)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(935, 935),  31, new Vector2(925, 945)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(675, 820),  31, new Vector2(675, 820)));
            
//          Top of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(620, 835), 38, new Vector2(625, 845)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(485, 910), 38, new Vector2(475, 920)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(495, 920), 37, new Vector2(475, 920)));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(275, 1020),37, new Vector2(275, 1020)));
        }

        public static void CreateBicycleLights()
        {
//          Bottom of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(825, 1195), 22, new Vector2(825, 1195)));
//          Top of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(625, 845), 24, new Vector2(625, 845)));
//          Right of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(1075, 1020), 21, new Vector2(1075, 1020)));
//          Left of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(225, 1045), 23, new Vector2(225, 1045)));
            
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
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,250), "Show PedestrianLightIds",DebugOptions.DrawPedestrianLightIds, new Vector2(-180, 15), "PedestrianLightIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,300), "Turn on the logger?",    DebugOptions.Logging,                new Vector2(-180, 15), "Logger"));
            
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(300, 650), ButtonStates.DebugPlayButton, "PlayButton"));
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(400, 650), ButtonStates.Replay, "ReplayButton"));
        }
    }
}