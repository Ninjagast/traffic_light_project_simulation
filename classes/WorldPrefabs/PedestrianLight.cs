using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class PedestrianLight: IDrawAble
    {
        private int _laneId;
        private int _currentFrame = 0;
        private States _state;
        private Vector2 _pos;
        private SpriteFont _font;
        
        public void Update()
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 20), Color.Black);

            if (_state == States.Orange)
            {
                _currentFrame += 1;
                if (_currentFrame < 30)
                {
                    spriteBatch.Draw(TextureManager.Instance.GetTexture(3, "PeopleGreen"), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
                }
                else if (_currentFrame > 59)
                {
                    _currentFrame = 0;
                }
            }
            else
            {
                spriteBatch.Draw(TextureManager.Instance.GetTexture(3, "People" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            }
        }

        public static PedestrianLight CreateInstance(Vector2 pos, int routeId, SpriteFont font)
        {
            PedestrianLight returnInstance = new PedestrianLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._state = States.Red;
            returnInstance._font = font;
            return returnInstance;
        }
        
        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }
    }
}