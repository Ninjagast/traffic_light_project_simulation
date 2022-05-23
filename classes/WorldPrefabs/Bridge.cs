using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class Bridge: IDrawAble
    {
        private int _bridgeId; //we support multiple bridges broker does not 
        private Vector2 _pos;
        private States _state = States.Closed;
        public void Update()
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_state == States.Closed)
            {
                spriteBatch.Draw(TextureManager.Instance.GetTexture("bridgeClosed"), _pos, Color.White);
            }
            else
            {
                spriteBatch.Draw(TextureManager.Instance.GetTexture("bridgeOpen"), _pos, Color.White);
            }
        }

        public void StateChange(int id, States state)
        {
            if (_bridgeId == id)
            {
                _state = state;
            }
        }

        public void DrawId(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(),$"{_bridgeId}", _pos, Color.Black);
        }

        public void DrawTargetArea(SpriteBatch spriteBatch)
        {
            return;
        }
        
        public static Bridge CreateInstance(Vector2 pos)
        {
            return new Bridge{_pos = pos};
        }
    }
}