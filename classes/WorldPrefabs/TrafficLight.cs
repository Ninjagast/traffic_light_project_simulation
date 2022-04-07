using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class TrafficLight: IDrawAble
    {
        private Texture2D _textureRed;
        private Texture2D _textureOrange;
        private Texture2D _textureGreen;
        private int _laneId;
        private Vector2 _pos;
        private States _state; 
        private SpriteFont _font;

        public void Update()
        {
//          todo look if there is a car in the waypoint for this traffic light so we can stop the car and send a signal to the controller
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_state == States.GREEN)
            {
                spriteBatch.Draw(_textureGreen, new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            }
            else if (_state == States.ORANGE)
            {
                spriteBatch.Draw(_textureOrange, new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            }
            else
            {
                spriteBatch.Draw(_textureRed, new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            }

            spriteBatch.DrawString(_font, _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 10), Color.Black);
        }

        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }

        public static TrafficLight CreateInstance(Vector2 pos, int routeId, Texture2D textureRed, Texture2D textureOrange, Texture2D textureGreen, SpriteFont font)
        {
            TrafficLight returnInstance = new TrafficLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._textureGreen = textureGreen;
            returnInstance._textureOrange = textureOrange;
            returnInstance._textureRed = textureRed;
            returnInstance._state = States.RED;
            returnInstance._font = font;
            return returnInstance;
        }
    }
}