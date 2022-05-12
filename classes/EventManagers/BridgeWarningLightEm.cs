using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.EventManagers
{
    public class BridgeWarningLightEm: IEventManager
    {
        private static BridgeWarningLightEm _instance;
        private List<IDrawAble> _subscribed = new List<IDrawAble>();
        private static readonly object Padlock = new object();
        private BridgeWarningLightEm() {}
        public static BridgeWarningLightEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new BridgeWarningLightEm();
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
            foreach (var iDrawAble in _subscribed)
            {
                iDrawAble.StateChange(id, state);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var iDrawAble in _subscribed)
            {
                iDrawAble.Draw(spriteBatch);
            }
        }

        public void DebugDrawIds(SpriteBatch spriteBatch)
        {
            foreach (var drawAble in _subscribed)
            {
                drawAble.DrawId(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var iDrawAble in _subscribed)
            {
                iDrawAble.Update();
            }
        }

        public void DebugDrawTargetAreas(SpriteBatch spriteBatch)
        {
            foreach (var drawAble in _subscribed)
            {
                drawAble.DrawTargetArea(spriteBatch);
            }
        }
    }
}
