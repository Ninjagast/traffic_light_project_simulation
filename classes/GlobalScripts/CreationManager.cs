using System.Collections.Generic;
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
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(925, 1145), 33, new Vector2(925, 1145), "RIGHT", new List<int>{33}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(1125, 1045),33, new Vector2(1125, 1045),"LEFT", new List<int>{33, 34}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(825, 1195), 34, new Vector2(825, 1195), "RIGHT", new List<int>{33, 34}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(915, 1130), 34, new Vector2(925, 1145), "LEFT", new List<int>{34}));
            
//          Left of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(700, 1260), 35, new Vector2(625, 1245), "UP", new List<int>{35, 36}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(435, 1135), 35, new Vector2(475, 1170), "DOWN", new List<int>{35}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(425, 1145), 36, new Vector2(425, 1145), "UP", new List<int>{36}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(235, 1040), 36, new Vector2(275, 1070), "DOWN", new List<int>{36, 35}));
            
//          Right of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(935, 935),  31, new Vector2(875, 920),  "UP", new List<int>{31}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(675, 820),  31, new Vector2(675, 820),  "DOWN", new List<int>{31, 32}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(1085,1010), 32, new Vector2(1075, 1020),"UP", new List<int>{32, 31}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(925, 945),  32, new Vector2(925, 945),  "DOWN", new List<int>{32}));
            
//          Top of the intersection
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(485, 910), 37, new Vector2(475, 920), "LEFT", new List<int>{37}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(275, 1020),37, new Vector2(275, 1020),"RIGHT", new List<int>{37, 38}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(620, 835), 38, new Vector2(625, 845), "LEFT", new List<int>{38, 37}));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(495, 920), 38, new Vector2(475, 920), "RIGHT", new List<int>{38}));
        }

        public static void CreateBicycleLights()
        {
//          Bottom of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(825, 1195), 22, new Vector2(825, 1195), "RIGHT"));
//          Top of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(575, 870), 24, new Vector2(575, 870), "LEFT"));
//          Right of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(1025, 995), 21, new Vector2(1025, 995), "UP"));
//          Left of the intersection
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(275, 1070), 23, new Vector2(275, 1070), "DOWN"));
            
        }

        public static void CreateBridges()
        {
            BridgeEm.Instance.Subscribe(Bridge.CreateInstance(new Vector2(2204, 204)));
        }

        public static void CreateHitTrees()
        {
            BridgeHitTreeEm.Instance.Subscribe(HitTree.CreateInstance(new Vector2(2300, 30), new Dictionary<string, Vector2>(){{"car", new Vector2(2375, 220)}, {"notCar", new Vector2(2325, 195)}}, "LEFT", "DOWN"));
            BridgeHitTreeEm.Instance.Subscribe(HitTree.CreateInstance(new Vector2(2550, 175), new Dictionary<string, Vector2>(), "", "UP"));
            
            BridgeHitTreeEm.Instance.Subscribe(HitTree.CreateInstance(new Vector2(2200, 100), new Dictionary<string, Vector2>(), "", "DOWN"));
            BridgeHitTreeEm.Instance.Subscribe(HitTree.CreateInstance(new Vector2(2425, 235), new Dictionary<string, Vector2>{{"car", new Vector2(2325, 295)}, {"notCar", new Vector2(2375,320)}}, "RIGHT", "UP"));
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
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,50),  "Show car claimed cells",     DebugOptions.DrawCarClaimedCells,     new Vector2(-190,15),  "CarClaimedCells"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,100), "Show bike claimed cells",    DebugOptions.DrawBikeClaimedCells,    new Vector2(-190,15),  "BikeClaimedCells"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,150), "Show people claimed cells",  DebugOptions.DrawPeopleClaimedCells,  new Vector2(-190,15),  "PeopleClaimedCells"));
                       
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(500,50),  "Show TrafficLight target cells",    DebugOptions.DrawTrafficLightTargetArea,    new Vector2(-250,15),  "TrafficLightTargetArea"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(500,100), "Show BikeLight target cells",       DebugOptions.DrawBicycleLightTargetArea,    new Vector2(-250,15),  "BikeLightTargetArea"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(500,150), "Show PedestrianLight claimed cells",DebugOptions.DrawPedestrianLightTargetArea, new Vector2(-250,15),  "PedestrianTargetArea"));
            
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,250), "Show all entity ids",    DebugOptions.DrawCarIds,             new Vector2(-190,15),  "CarIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,300), "Show TrafficLightIds",   DebugOptions.DrawTrafficLightIds,    new Vector2(-190,15),  "TrafficLightIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,350), "Show BicycleLightIds",   DebugOptions.DrawBicycleLightIds,    new Vector2(-190,15),  "BicycleLightIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,400), "Show PedestrianLightIds",DebugOptions.DrawPedestrianLightIds, new Vector2(-190, 15), "PedestrianLightIds"));
            UiHandler.Instance.Subscribe(new CheckBox(new Vector2(200,500), "Turn on the logger?",    DebugOptions.Logging,                new Vector2(-190, 15), "Logger"));
            
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(300, 650), ButtonStates.DebugPlayButton, "PlayButton"));
            UiHandler.Instance.Subscribe(new BaseButton(new Vector2(400, 650), ButtonStates.Replay, "ReplayButton"));
        }
    }
}