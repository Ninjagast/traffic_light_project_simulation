using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class Car: IDrawAble
    {
        private Vector2 _pos;
        private States _state;
        private DirectionMap _directionMap;
        private Dictionary<string, Vector2> _orientation;
        
        private string _lastDirection;
        private int _currentFrame = 0;
        private int _id = -1;
        private int _step = 0;
        private int _repetition = 0;
        private int _speed;
        private bool _claimedStandingCell = false;
        private int _reaction = 0;
        private int _waiting = 0;

        public void Update()
        {
            if(_state == States.Transit)
            {
                if (_waiting >= _reaction)
                {
                    if (_currentFrame == 1)
                    {
                        VehicleEm.Instance.UnClaimCell(_pos - _orientation[_lastDirection]);
                    }
                    _currentFrame++;
                    _pos += (_orientation[_lastDirection]);
                    if (_currentFrame == (50 / _speed))
                    {
                        _claimedStandingCell = false;
                        _state = States.Driving;
                        // Console.WriteLine("current position");
                        // Console.WriteLine(_pos);
                        _currentFrame = 0;
                    }
                }
                else
                {
                    _waiting++;
                }
            }
            else if (_state == States.Driving)
            {
                if (_repetition < _directionMap.directions[_step].repeat)
                {          

                    Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step].direction] * ((50 / _speed) - _currentFrame));
                    if (VehicleEm.Instance.IsCellFree(targetPos))
                    {
                        VehicleEm.Instance.ClaimCell(targetPos, _id);        
                        _claimedStandingCell = false;
                        _state = States.Transit;
                        _lastDirection = _directionMap.directions[_step].direction;
                        _repetition++;
                    }
                    else
                    {
                        _waiting = 0;
                    }
                }
                else
                {
                    _step++;
                    _repetition = 1;
//                  delete this car if we have done the last step
                    if ((_step >= _directionMap.directions.Count) || (_directionMap.directions.Count == 1 && _repetition == _directionMap.directions[0].repeat))
                    {
                        VehicleEm.Instance.UnClaimCell(_pos); 
                        VehicleEm.Instance.UnSubscribe(_id); //todo might create a memory leak
                        return;
                    }
                    
                    Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step].direction] * ((50 / _speed) - _currentFrame));
                    if (VehicleEm.Instance.IsCellFree(targetPos))
                    {
                        VehicleEm.Instance.ClaimCell(targetPos, _id);  
                        _claimedStandingCell = false;
                        _state = States.Transit;
                        _lastDirection = _directionMap.directions[_step].direction;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(0, "sedan_" + _lastDirection), new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
            spriteBatch.DrawString(TextureManager.Instance.getFont(), _id.ToString(), _pos,Color.Black);
        }

        public void StateChange(int id, States state)
        {
            if (_id == id)
            {
                if (state == States.Transit)
                {
                    _waiting = 0;
                    VehicleEm.Instance.UnClaimCell(_pos);
                }
                _state = state;
            }
        }

        public static Car CreateInstance(bool testing, Random random)
        {
            VehicleEm.Instance.GetNextId();
            Car returnObject = new Car();
            if (testing)
            {
                returnObject._directionMap = SpawnPoints.Instance.GetFromTestRoutes();
                if (returnObject._directionMap == null)
                {
                    return null;
                }
                returnObject._pos = new Vector2(returnObject._directionMap.vector2.x, returnObject._directionMap.vector2.y);
                if (!VehicleEm.Instance.IsCellFree(returnObject._pos))
                {
                    return null;
                }
            }
            else
            {
//              todo can be put into a while loop in order to not skip a spawn chance when it occurs
                returnObject._directionMap = SpawnPoints.Instance.GetRandomLandSpawnPoint();
                returnObject._pos = new Vector2(returnObject._directionMap.vector2.x, returnObject._directionMap.vector2.y);
                if (!VehicleEm.Instance.IsCellFree(returnObject._pos))
                {
                    return null;
                }
            }
            returnObject._state = States.Driving;
            returnObject._currentFrame = 0;
            returnObject._orientation = new Dictionary<string, Vector2>
            {
                {"LEFT", new Vector2(-1, 0.5f) * VehicleEm.Instance.Speed},
                {"RIGHT", new Vector2(1, -0.5f) * VehicleEm.Instance.Speed},
                {"DOWN", new Vector2(1, 0.5f) * VehicleEm.Instance.Speed},
                {"UP", new Vector2(-1, -0.5f) * VehicleEm.Instance.Speed}
            };
            returnObject._lastDirection = returnObject._directionMap.directions[0].direction;
            returnObject._id = VehicleEm.Instance.GetNextId();
            returnObject._speed = VehicleEm.Instance.Speed;
            returnObject._reaction = random.Next(0, 10);
            VehicleEm.Instance.ClaimCell(returnObject._pos, returnObject._id);        
            return returnObject;
        }
    }
}