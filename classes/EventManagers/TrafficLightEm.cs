using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.EventManagers
{
    public class TrafficLightEm: IEventManager
    {
        private static TrafficLightEm _instance;
        private List<IDrawAble> _subscribed = new List<IDrawAble>();
        private static readonly object Padlock = new object();
        private TrafficLightEm() {}
        public static TrafficLightEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TrafficLightEm();
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
    }
}