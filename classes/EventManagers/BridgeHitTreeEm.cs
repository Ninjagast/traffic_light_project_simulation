using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.EventManagers
{
    public class BridgeHitTreeEm: IEventManager
    {
        private static BridgeHitTreeEm _instance;
        private List<HitTree> _subscribed = new List<HitTree>();
        private static readonly object Padlock = new object();

        private BridgeHitTreeEm() {}
        
        public static BridgeHitTreeEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new BridgeHitTreeEm();
                    }
                    return _instance;
                }
            }
        }

        public void Subscribe(IDrawAble drawAble)
        {
            throw new NotImplementedException();
        }

        public void OnStateChange(int id, States state)
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}