using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.EventManagers
{
    public class BridgeHitTreeEm: IEventManager
    {
        private static BridgeHitTreeEm _instance;
        private List<IDrawAble> _subscribed = new List<IDrawAble>();
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
            _subscribed.Add(drawAble);
        }

        public void OnStateChange(int id, States state)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.StateChange(id, state);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Draw(spriteBatch);
            }
        }
        public void DebugDrawIds(SpriteBatch spriteBatch)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.DrawId(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Update();
            }
        }
    }
}