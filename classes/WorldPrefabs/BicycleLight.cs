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
        private int _stoppedBikeId = -1;
        private string _direction;
        
        public void Update()
        {
            if (_state == States.Red || _state == States.Orange)
            {
                int id = VehicleEm.Instance.GetBikeId(_targetArea, _direction);
                if(id > -1 && _stoppedBikeId == -1)
                {
                    _stoppedBikeId = id;
                    VehicleEm.Instance.OnStateChange(id, States.Idle);
                    Server.Instance.EntityEnteredZone(_laneId);
                }
            }
            else if (_state == States.Green)
            {
                if (_stoppedBikeId > -1)
                {
                    VehicleEm.Instance.OnStateChange(_stoppedBikeId, States.Transit);
                    Server.Instance.EntityExitedZone(_laneId);
   
                    _stoppedBikeId = -1;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture("Bike" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
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

        public void DrawTargetArea(SpriteBatch spriteBatch)
        {
            Vector2 offset = Vector2.Zero;
            switch (_direction)
            {
                case "RIGHT":
                    offset = new Vector2(50, 0);
                    break;
                case "LEFT":
                    offset = Vector2.Zero;
                    break;
                case "DOWN":
                    offset = new Vector2(23, 13);
                    break;
                case "UP":
                    offset = new Vector2(22, -10);
                    break;
            }
            spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                new Rectangle((int) ((_targetArea.X - 25) + offset.X), (int) ((_targetArea.Y + 15) + offset.Y), 50, 25), Color.Yellow);
        }

        public static BicycleLight CreateInstance(Vector2 pos, int routeId, Vector2 targetArea, string direction)
        {
            BicycleLight returnInstance = new BicycleLight
            {
                _laneId = routeId, _pos = pos, _state = States.Red, _targetArea = targetArea, _direction = direction
            };
            return returnInstance;
        }
    }
}