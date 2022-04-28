using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.debug
{
    public class DebugManager: IEventManager
    {
        private static DebugManager _instance;
        private static readonly object Padlock = new object();
        private DebugManager() {}
        public static DebugManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DebugManager();
                    }
                    return _instance;
                }
            }
        }

        private List<DebugOptions> _debugOptions = new List<DebugOptions>();

        public bool Logging { get; set; } = false;
        public int UpdateTick { get; set; } = 0;

        public void Subscribe(IDrawAble drawAble)
        {
            throw new System.NotImplementedException();
        }

        public void OnStateChange(int id, States state)
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
//          draws debug info based on selected options (Each prefab knows how to do this themselves)
            foreach (DebugOptions option in _debugOptions)
            {
                switch (option)
                {
                    case DebugOptions.DrawClaimedCells:
                        VehicleEm.Instance.DebugDrawMarkers(spriteBatch);
                        break;
                    
                    case DebugOptions.DrawCarIds:
                        VehicleEm.Instance.DebugDrawIds(spriteBatch);
                        break;
                    
                    case DebugOptions.DrawBicycleLightIds:
                        break;
                    
                    case DebugOptions.DrawPedestrianLightIds:
                        break;
                    
                    case DebugOptions.DrawTrafficLightIds:
                        TrafficLightEm.Instance.DebugDrawIds(spriteBatch);
                        break;
                    
                    default:
                        break;
                }
            }
        }
        
        public void Update()
        {
            return;
        }

        public void AddDebugOption(DebugOptions option)
        {
            if (!_debugOptions.Contains(option))
            {
                _debugOptions.Add(option);
            }
        }

        public void RemoveDebugOption(DebugOptions option)
        {
            _debugOptions.Remove(option);
        }

        public void SetUp()
        {
            if (_debugOptions.Contains(DebugOptions.Logging))
            {
                Logging = true;
                Logger.Instance.SetUp();
            }
        }
    }
}