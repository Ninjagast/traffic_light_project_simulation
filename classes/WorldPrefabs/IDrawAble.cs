using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public interface IDrawAble
    {
        public void Update();
        public void Draw(SpriteBatch spriteBatch);
        public void StateChange(int id, States state);
        public void DrawId(SpriteBatch spriteBatch);
        public void DrawTargetArea(SpriteBatch spriteBatch);
    }
}