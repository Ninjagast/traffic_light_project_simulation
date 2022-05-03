using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class PedestrianLight: IDrawAble
    {
        private int _laneId;
        private Vector2 _pos;
        private States _state; 
        private Vector2 _targetArea;
        private int _stoppedCarId = -1;
        private int _currentFrame = 0;
        private string _direction;
        
        public void Update()
        {
            if (_state == States.Red || _state == States.Orange)
            {
                int id = VehicleEm.Instance.GetCellPeopleId(_targetArea, _direction);
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
        
        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }

        public void DrawId(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 20), Color.Black);
        }

        public void DrawTargetArea(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                new Rectangle((int) _targetArea.X - 22, (int) _targetArea.Y + 12, 99, 50), Color.Aqua);
        }

        public static PedestrianLight CreateInstance(Vector2 pos, int routeId, Vector2 targetArea, string direction)
        {
            PedestrianLight returnInstance = new PedestrianLight
            {
                _laneId = routeId, _pos = pos, _state = States.Red, _targetArea = targetArea, _direction = direction
            };
            return returnInstance;
        }
        
    }
}