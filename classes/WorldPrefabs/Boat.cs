using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.dataClasses.ServerRequestData;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class Boat: IDrawAble
    {
        private readonly int _framesTillDone = 100;
        private string _currentDirection;
        private string _lastDirection;
        private int _currentFrame;
        private int _id;
        private int _step;
        private int _repetition;
        private int _speed;

        private Vector2 _pos;
        private States _state;
        private DirectionMap _directionMap;
        private Dictionary<string, Vector2> _orientation;
        
        public void Update()
        {
            if(_state == States.Transit)
            {
                if (_currentFrame == 3)
                {
                    if (_repetition == 1 && _step != 0)
                    {
                        VehicleEm.Instance.UnClaimBoatCell(_pos - (_orientation[_currentDirection] * 3), _lastDirection);
                    }
                    else
                    {
                        VehicleEm.Instance.UnClaimBoatCell(_pos - (_orientation[_currentDirection] * 3), _currentDirection);
                    }
                }
                _currentFrame++;
                _pos += (_orientation[_currentDirection]);
                if (_currentFrame == (_framesTillDone / _speed))
                {
                    _state = States.Driving;
                    _currentFrame = 0;
                }
            }
            else if (_state == States.Driving)
            {
//              if have not yet repeated this step enough time
                if (_repetition < _directionMap.directions[_step].repeat)
                {          
                    Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step].direction] * _framesTillDone / _speed);
                    if (VehicleEm.Instance.IsBoatCellFree(targetPos, _currentDirection))
                    {
                        VehicleEm.Instance.ClaimBoatCell(targetPos, _id, _directionMap.directions[_step].direction);        
                        _state = States.Transit;
                        _currentDirection = _directionMap.directions[_step].direction;
                        _repetition++;
                        _currentFrame++;
                        _pos += (_orientation[_currentDirection]);
                    }
                }
                else
                {
//                  delete this person if we have done the last step
                    if ((_step + 1 >= _directionMap.directions.Count) || (_directionMap.directions.Count == 1 && _repetition == _directionMap.directions[0].repeat))
                    {
                        VehicleEm.Instance.UnClaimBoatCell(_pos, _currentDirection); 
                        VehicleEm.Instance.UnSubscribe(_id); //todo might create a memory leak
                    }
                    else
                    {
                        Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step + 1].direction] * _framesTillDone / _speed);
                        if (VehicleEm.Instance.IsBoatCellFree(targetPos, _directionMap.directions[_step + 1].direction))
                        {
                            VehicleEm.Instance.ClaimBoatCell(targetPos, _id, _directionMap.directions[_step + 1].direction);  
                            _lastDirection = _directionMap.directions[_step].direction;
                            _state = States.Transit;
                            _step++;
                            _repetition = 1;
                            _currentDirection = _directionMap.directions[_step].direction;
                            _currentFrame++;
                            _pos += (_orientation[_currentDirection]);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture("boat_small_" + _currentDirection), new Rectangle((int)_pos.X - 5, (int)_pos.Y + 15, 68, 40), Color.White);
        }

        public void StateChange(int id, States state)
        {
            if (id == _id)
            {
                _state = state;
            }
        }

        public void DrawId(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _id.ToString(), _pos,Color.Black);
        }

        public void DrawTargetArea(SpriteBatch spriteBatch)
        {
            return;
        }
        
        public static Boat CreateInstance(Random random)
        {
            DirectionMap map = null;

            int i = 0;

            while (10 > i)
            {
                map = WeightTableHandler.Instance.GetRandomSeaRoute();
                if (VehicleEm.Instance.IsBoatCellFree(new Vector2(map.vector2.x, map.vector2.y), map.directions[0].direction))
                {
                    break;
                }
                else if (i == 9)
                {
//                  Not a single position was available (from the ones we checked)
                    return null;
                }
                i++;
            }

            Boat returnObject = new Boat
            {
                _directionMap = map,
                _pos = new Vector2(map.vector2.x, map.vector2.y),
                _state = States.Driving,
                _currentFrame = 0,
                _orientation = new Dictionary<string, Vector2>
                {
                    {"LEFT",  new Vector2(-1, 0.5f)  * 1f},
                    {"RIGHT", new Vector2(1, -0.5f)  * 1f},
                    {"DOWN",  new Vector2(1, 0.5f)   * 1f},
                    {"UP",    new Vector2(-1, -0.5f) * 1f}
                },
                _currentDirection = map.directions[0].direction,
                _id = VehicleEm.Instance.GetNextId(),
                _speed = VehicleEm.Instance.DefaultSpeed,
            };

            if (DebugManager.Instance.Logging)
            {
                Logger.Instance.LogEntitySpawn(new DebugLogEntitySpawn
                {
                    Tick = DebugManager.Instance.UpdateTick,
                    DirectionMap = map,
                    EntityType = "Boat"
                });
            }
            VehicleEm.Instance.ClaimBoatCell(returnObject._pos, returnObject._id, returnObject._currentDirection); 
            return returnObject;
        }

        public static Boat CreateReplayInstance(DirectionMap directionMap)
        {
            Boat returnObject = new Boat
            {
                _directionMap = directionMap,
                _pos = new Vector2(directionMap.vector2.x, directionMap.vector2.y),
                _state = States.Driving,
                _currentFrame = 0,
                _orientation = new Dictionary<string, Vector2>
                {
                    {"LEFT",  new Vector2(-1, 0.5f)  * 1f},
                    {"RIGHT", new Vector2(1, -0.5f)  * 1f},
                    {"DOWN",  new Vector2(1, 0.5f)   * 1f},
                    {"UP",    new Vector2(-1, -0.5f) * 1f}
                },
                _currentDirection = directionMap.directions[0].direction,
                _id = VehicleEm.Instance.GetNextId(),
                _speed = VehicleEm.Instance.DefaultSpeed,
            };

            VehicleEm.Instance.ClaimBoatCell(returnObject._pos, returnObject._id, returnObject._currentDirection);        
            
            return returnObject;
        }
    }
}