using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.EventManagers
{
    public class BridgeEm: IEventManager
    {
        private static BridgeEm _instance;
        private static readonly object Padlock = new object();
        private BridgeEm() {}
        public static BridgeEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new BridgeEm();
                    }
                    return _instance;
                }
            }
        }
        
        private List<IDrawAble> _subscribed = new List<IDrawAble>();


        public void Subscribe(IDrawAble drawAble)
        {
            _subscribed.Add(drawAble);
        }

        public void OnStateChange(int id, States state)
        {
            foreach (var drawAble in _subscribed)
            {
                drawAble.StateChange(id, state);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var drawAble in _subscribed)
            {
                drawAble.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            return;
        }
    }
}