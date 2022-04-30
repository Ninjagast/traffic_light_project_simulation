using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class BicycleLight : IDrawAble
    {
        
        private int _laneId;
        private Vector2 _pos;
        private States _state; 
        private Vector2 _targetArea;
        private int _stoppedCarId = -1;
        
        public void Update()
        {
            if (_state == States.Red || _state == States.Orange)
            {
                int id = VehicleEm.Instance.GetBikeId(_targetArea);
                if(id > -1 && _stoppedCarId == -1)
                {
                    _stoppedCarId = id;
                    VehicleEm.Instance.OnStateChange(id, States.Stopping);
                    Server.Instance.EntityEnteredZone(_laneId);
                }
            }
            else if (_state == States.Green)
            {
                if (_stoppedCarId > -1)
                {
                    VehicleEm.Instance.OnStateChange(_stoppedCarId, States.Driving);
                    Server.Instance.EntityExitedZone(_laneId);
   
                    _stoppedCarId = -1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(2, "Bike" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
        }

        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }

        public void DrawId(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 10), Color.Black);
        }

        public static BicycleLight CreateInstance(Vector2 pos, int routeId, Vector2 targetArea)
        {
            BicycleLight returnInstance = new BicycleLight
            {
                _laneId = routeId, _pos = pos, _state = States.Red, _targetArea = targetArea
            };
            return returnInstance;
        }
    }
}