using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.EventManagers
{
    public interface IEventManager
    {
        public void Subscribe(IDrawAble drawAble);
        public void OnStateChange(int id, States state);
        public void Draw(SpriteBatch spriteBatch);
        public void Update();
    }
}