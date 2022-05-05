using System.Collections.Generic;
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
        private List<int> _targetLanes;
        private Vector2 _pos;
        private States _state; 
        private Vector2 _targetArea;
        private int _stoppedPersonId = -1;
        private int _currentFrame = 0;
        private string _direction;
        
        public void Update()
        {
            if (_state == States.Red || _state == States.Orange)
            {
                int id = VehicleEm.Instance.GetCellPeopleId(_targetArea, _direction);
                if(id > -1 && _stoppedPersonId == -1)
                {
                    _stoppedPersonId = id;
                    VehicleEm.Instance.OnStateChange(id, States.Idle);
                    foreach (var lane in _targetLanes)
                    {
                        Server.Instance.EntityEnteredZone(lane);
                    }
                }
            }
            else if (_state == States.Green)
            {
                if (_stoppedPersonId > -1)
                {
                    VehicleEm.Instance.OnStateChange(_stoppedPersonId, States.Transit);
                    foreach (var lane in _targetLanes)
                    {
                        Server.Instance.EntityExitedZone(lane);
                    }
   
                    _stoppedPersonId = -1;
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
                    spriteBatch.Draw(TextureManager.Instance.GetTexture("PeopleGreen"), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
                }
                else if (_currentFrame > 59)
                {
                    _currentFrame = 0;
                }
            }
            else
            {
                spriteBatch.Draw(TextureManager.Instance.GetTexture("People" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
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
                new Rectangle((int) ((_targetArea.X - 25) + offset.X), (int) ((_targetArea.Y + 15) + offset.Y), 50, 25), Color.Aqua);
        }

        public static PedestrianLight CreateInstance(Vector2 pos, int routeId, Vector2 targetArea, string direction, List<int> targetLanes)
        {
            PedestrianLight returnInstance = new PedestrianLight
            {
                _laneId = routeId, _pos = pos, _state = States.Red, _targetArea = targetArea, _direction = direction, _targetLanes = targetLanes
            };
            return returnInstance;
        }
    }
}