using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class BicycleLight : IDrawAble
    {
        
        private int _laneId;
        private Vector2 _pos;
        private States _state; 
        private SpriteFont _font;

        public void Update()
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(2, "Bike" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            spriteBatch.DrawString(_font, _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 10), Color.Black);
        }

        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }

        public static BicycleLight CreateInstance(Vector2 pos, int routeId, SpriteFont font)
        {
            BicycleLight returnInstance = new BicycleLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._state = States.Red;
            returnInstance._font = font;
            return returnInstance;
        }
    }
}