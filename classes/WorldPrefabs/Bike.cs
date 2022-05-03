﻿using System;
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
    public class Bike: IDrawAble
    {
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
                        VehicleEm.Instance.UnClaimBikeCell(_pos - (_orientation[_currentDirection] * 3), _lastDirection);
                    }
                    else
                    {
                        VehicleEm.Instance.UnClaimBikeCell(_pos - (_orientation[_currentDirection] * 3), _currentDirection);
                    }
                }
                _currentFrame++;
                _pos += (_orientation[_currentDirection]);
                if (_currentFrame == (100 / _speed))
                {
                    _state = _state == States.Stopping ? States.Idle : States.Driving;
                    _currentFrame = 0;
                }
            }
            else if (_state == States.Driving)
            {
//              if have not yet repeated this step enough time
                if (_repetition < _directionMap.directions[_step].repeat)
                {          
                    Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step].direction] * 100 / _speed);
                    if (VehicleEm.Instance.IsBikeCellFree(targetPos, _currentDirection))
                    {
                        VehicleEm.Instance.ClaimBikeCell(targetPos, _id, _currentDirection);        
                        _state = States.Transit;
                        _repetition++;
                        _currentFrame++;
                        _pos += (_orientation[_currentDirection]);
                    }
                }
                else
                {
//                  delete this bike if we have done the last step
                    if ((_step + 1 >= _directionMap.directions.Count) || (_directionMap.directions.Count == 1 && _repetition == _directionMap.directions[0].repeat))
                    {
                        Console.WriteLine($"{_pos} last pos");
                        VehicleEm.Instance.UnClaimBikeCell(_pos, _currentDirection); 
                        VehicleEm.Instance.UnSubscribe(_id); //todo might create a memory leak
                    }
                    else
                    {
                        Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step + 1].direction] * 100 / _speed);
                        if (VehicleEm.Instance.IsBikeCellFree(targetPos, _directionMap.directions[_step + 1].direction))
                        {
                            VehicleEm.Instance.ClaimBikeCell(targetPos, _id, _directionMap.directions[_step + 1].direction);
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
            spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("fietser"), new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
        }

        public void StateChange(int id, States state)
        {
            if (_id == id)
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

        public static Bike CreateInstance(Random random)
        {
            DirectionMap map = null;

            int i = 0;

            while (10 > i)
            {
                map = WeightTableHandler.Instance.GetRandomSideWalkRoute();
                if (VehicleEm.Instance.IsBikeCellFree(new Vector2(map.vector2.x, map.vector2.y), map.directions[0].direction))
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

            Bike returnObject = new Bike
            {
                _directionMap = map,
                _pos = new Vector2(map.vector2.x, map.vector2.y),
                _state = States.Driving,
                _currentFrame = 0,
                _orientation = new Dictionary<string, Vector2>
                {
                    {"LEFT",  new Vector2(-1, 0.5f)},
                    {"RIGHT", new Vector2(1, -0.5f)},
                    {"DOWN",  new Vector2(1, 0.5f)},
                    {"UP",    new Vector2(-1, -0.5f)}
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
                    EntityType = "Bike"
                });
            }
            VehicleEm.Instance.ClaimBikeCell(returnObject._pos, returnObject._id, returnObject._currentDirection); 
            return returnObject;
        }

        public static Bike CreateReplayInstance(DirectionMap directionMap)
        {
            Bike returnObject = new Bike
            {
                _directionMap = directionMap,
                _pos = new Vector2(directionMap.vector2.x, directionMap.vector2.y),
                _state = States.Driving,
                _currentFrame = 0,
                _orientation = new Dictionary<string, Vector2>
                {
                    {"LEFT",  new Vector2(-1, 0.5f)},
                    {"RIGHT", new Vector2(1, -0.5f)},
                    {"DOWN",  new Vector2(1, 0.5f)},
                    {"UP",    new Vector2(-1, -0.5f)}
                },
                _currentDirection = directionMap.directions[0].direction,
                _id = VehicleEm.Instance.GetNextId(),
                _speed = VehicleEm.Instance.DefaultSpeed,
            };

            VehicleEm.Instance.ClaimBikeCell(returnObject._pos, returnObject._id, returnObject._currentDirection);        

            return returnObject;
        }
    }
}