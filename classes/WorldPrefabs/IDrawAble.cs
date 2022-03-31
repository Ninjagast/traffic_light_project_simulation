using System;
using Microsoft.Xna.Framework.Graphics;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public interface IDrawAble
    {
        public void Update();
        public void Draw(SpriteBatch spriteBatch);
        public void StateChange(int id, Enum state);
    }
}