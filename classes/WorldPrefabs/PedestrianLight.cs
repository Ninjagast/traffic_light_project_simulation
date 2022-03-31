using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.EventManagers
{
    public class PedestrianLight: IDrawAble
    {
        private Texture2D _textureGreen;
        private Texture2D _textureRed;
        private int _laneId;
        private int _currentFrame = 0;
        private States _state;
        private Vector2 _pos;
        private SpriteFont _font;

        
        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 20), Color.Black);

            if (_state == States.GREEN)
            {
                spriteBatch.Draw(_textureGreen, new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            }
            else if(_state == States.RED)
            {
                spriteBatch.Draw(_textureRed, new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            }
            else
            {
                _currentFrame += 1;
                if (_currentFrame < 30)
                {
                    spriteBatch.Draw(_textureGreen, new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
                }
                else if (_currentFrame > 59)
                {
                    _currentFrame = 0;
                    return;
                }
            }
        }

        public static PedestrianLight CreateInstance(Vector2 pos, int routeId, Texture2D textureRed, Texture2D textureGreen,  SpriteFont font)
        {
            PedestrianLight returnInstance = new PedestrianLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._textureGreen = textureGreen;
            returnInstance._textureRed = textureRed;
            returnInstance._state = States.RED;
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