using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;

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
        private Vector2 _targetArea;
        private int _stoppedCarId = -1;

        public void Update()
        {
//          todo look if there is a car in the waypoint for this traffic light so we can stop the car and send a signal to the controller
            if (_state == States.RED)
            {
                int id = VehicleEm.Instance.GetCellCarId(_targetArea);
                if(id > -1 && _stoppedCarId == -1)
                {
                    _stoppedCarId = id;
                    VehicleEm.Instance.OnStateChange(id, States.IDLE);
                    Server.Instance.EntityEnteredZone(_laneId);
                }
            }

            if (_stoppedCarId > -1)
            {
                if (_state == States.GREEN)
                {
                    _stoppedCarId = -1;
                    VehicleEm.Instance.OnStateChange(_stoppedCarId, States.DRIVING);
                    Server.Instance.EntityExitedZone(_laneId);
                }
            }
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

        public static TrafficLight CreateInstance(Vector2 pos, int routeId, Texture2D textureRed, Texture2D textureOrange, Texture2D textureGreen, SpriteFont font, Vector2 targetArea)
        {
            TrafficLight returnInstance = new TrafficLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._textureGreen = textureGreen;
            returnInstance._textureOrange = textureOrange;
            returnInstance._textureRed = textureRed;
            returnInstance._state = States.RED;
            returnInstance._font = font;
            returnInstance._targetArea = targetArea;
            return returnInstance;
        }
    }
}