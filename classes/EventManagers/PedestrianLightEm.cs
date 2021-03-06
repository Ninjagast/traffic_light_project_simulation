using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.EventManagers
{
    public class PedestrianLightEm: IEventManager
    {
        private static PedestrianLightEm _instance;
        private List<IDrawAble> _subscribed = new List<IDrawAble>();
        private static readonly object Padlock = new object();
        private PedestrianLightEm() {}
        public static PedestrianLightEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new PedestrianLightEm();
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
            foreach (var drawAble in _subscribed)
            {
                drawAble.DrawId(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Update();
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