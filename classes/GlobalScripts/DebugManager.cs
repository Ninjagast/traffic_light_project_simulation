using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.GlobalScripts
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

        private List<DebugOptions> _drawOptions = new List<DebugOptions>();

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
            foreach (DebugOptions option in _drawOptions)
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
                        Console.WriteLine($"{option} is not a valid debug draw option");
                        break;
                }
            }
        }

        public void DebugDrawIds(SpriteBatch spriteBatch)
        {
            return;
        }

        public void Update()
        {
            return;
        }

        public void AddDrawOption(DebugOptions option)
        {
            if (!_drawOptions.Contains(option))
            {
                _drawOptions.Add(option);
            }
        }

        public void RemoveDrawOption(DebugOptions option)
        {
            _drawOptions.Remove(option);
        }
    }
}