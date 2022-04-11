using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class TrafficLight: IDrawAble
    {
        private int _laneId;
        private Vector2 _pos;
        private States _state; 
        private SpriteFont _font;
        private Vector2 _targetArea;
        private int _stoppedCarId = -1;

        public void Update()
        {
            if (_state == States.Red)
            {
                int id = VehicleEm.Instance.GetCellCarId(_targetArea);
                if(id > -1 && _stoppedCarId == -1)
                {
                    _stoppedCarId = id;
                    VehicleEm.Instance.OnStateChange(id, States.Idle);
                    Server.Instance.EntityEnteredZone(_laneId);
                }
            }
            else if (_state == States.Green)
            {
                if (_stoppedCarId > -1)
                {
                    VehicleEm.Instance.OnStateChange(_stoppedCarId, States.Transit);
                    Server.Instance.EntityExitedZone(_laneId);
                    _stoppedCarId = -1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(1, "Light" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
            spriteBatch.DrawString(_font, _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 10), Color.Black);
        }

        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }

        public static TrafficLight CreateInstance(Vector2 pos, int routeId, SpriteFont font, Vector2 targetArea)
        {
            TrafficLight returnInstance = new TrafficLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._state = States.Red;
            returnInstance._font = font;
            returnInstance._targetArea = targetArea;
            return returnInstance;
        }
    }
}