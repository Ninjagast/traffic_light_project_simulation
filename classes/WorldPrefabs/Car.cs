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
    public class Car: IDrawAble
    {
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
                    VehicleEm.Instance.UnClaimCell(_pos - (_orientation[_lastDirection] * 3));
                }
                _currentFrame++;
                _pos += (_orientation[_lastDirection]);
                if (_currentFrame == (50 / _speed))
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
                    Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step].direction] * 50 / _speed);
                    if (VehicleEm.Instance.IsCellFree(targetPos))
                    {
                        VehicleEm.Instance.ClaimCell(targetPos, _id);        
                        _state = States.Transit;
                        _lastDirection = _directionMap.directions[_step].direction;
                        _repetition++;
                    }
                }
                else
                {
//                  delete this car if we have done the last step
                    if ((_step + 1 >= _directionMap.directions.Count) || (_directionMap.directions.Count == 1 && _repetition == _directionMap.directions[0].repeat))
                    {
                        DirectionMap newMap = WeightTableHandler.Instance.GetRandomExtension(_pos);
                        if (newMap == null)
                        {
                            Console.WriteLine($"My last position: {_pos}");
                            VehicleEm.Instance.UnClaimCell(_pos); 
                            VehicleEm.Instance.UnSubscribe(_id); //todo might create a memory leak
                        }
                        else
                        {
                            _step = 0;
                            _repetition = 0;
                            _directionMap = newMap;
                        }
                    }
                    else
                    {
                        Vector2 targetPos = _pos + (_orientation[_directionMap.directions[_step + 1].direction] * 50 / _speed);
                        if (VehicleEm.Instance.IsCellFree(targetPos))
                        {
                            VehicleEm.Instance.ClaimCell(targetPos, _id);  
                            _state = States.Transit;
                            _step++;
                            _repetition = 1;
                            _lastDirection = _directionMap.directions[_step].direction;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(0, "sedan_" + _lastDirection), new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
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

        public static Car CreateInstance(Random random)
        {
            DirectionMap map = null;

            int i = 0;

            while (10 > i)
            {
                map = WeightTableHandler.Instance.GetRandomRoute();
                if (VehicleEm.Instance.IsCellFree(new Vector2(map.vector2.x, map.vector2.y)))
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

            Car returnObject = new Car
            {
                _directionMap = map,
                _pos = new Vector2(map.vector2.x, map.vector2.y),
                _state = States.Driving,
                _currentFrame = 0,
                _orientation = new Dictionary<string, Vector2>
                {
                    {"LEFT",  new Vector2(-1, 0.5f)  * VehicleEm.Instance.DefaultSpeed},
                    {"RIGHT", new Vector2(1, -0.5f)  * VehicleEm.Instance.DefaultSpeed},
                    {"DOWN",  new Vector2(1, 0.5f)   * VehicleEm.Instance.DefaultSpeed},
                    {"UP",    new Vector2(-1, -0.5f) * VehicleEm.Instance.DefaultSpeed}
                },
                _lastDirection = map.directions[0].direction,
                _id = VehicleEm.Instance.GetNextId(),
                _speed = VehicleEm.Instance.DefaultSpeed,
            };

            if (DebugManager.Instance.Logging)
            {
                Logger.Instance.LogEntitySpawn(new DebugLogEntitySpawn
                {
                    Tick = DebugManager.Instance.UpdateTick,
                    DirectionMap = map,
                    EntityType = "Car"
                });
            }
            VehicleEm.Instance.ClaimCell(returnObject._pos, returnObject._id); 
            Console.WriteLine($"{returnObject._pos} start position");
            return returnObject;
        }

        public static Car CreateReplayInstance(DirectionMap directionMap)
        {
            Car returnObject = new Car
            {
                _directionMap = directionMap,
                _pos = new Vector2(directionMap.vector2.x, directionMap.vector2.y),
                _state = States.Driving,
                _currentFrame = 0,
                _orientation = new Dictionary<string, Vector2>
                {
                    {"LEFT",  new Vector2(-1, 0.5f)  * VehicleEm.Instance.DefaultSpeed},
                    {"RIGHT", new Vector2(1, -0.5f)  * VehicleEm.Instance.DefaultSpeed},
                    {"DOWN",  new Vector2(1, 0.5f)   * VehicleEm.Instance.DefaultSpeed},
                    {"UP",    new Vector2(-1, -0.5f) * VehicleEm.Instance.DefaultSpeed}
                },
                _lastDirection = directionMap.directions[0].direction,
                _id = VehicleEm.Instance.GetNextId(),
                _speed = VehicleEm.Instance.DefaultSpeed,
            };

            if (DebugManager.Instance.Logging)
            {
                Logger.Instance.LogEntitySpawn(new DebugLogEntitySpawn
                {
                    Tick = DebugManager.Instance.UpdateTick,
                    DirectionMap = directionMap,
                    EntityType = "car"
                });
            }
            VehicleEm.Instance.ClaimCell(returnObject._pos, returnObject._id);        
            
            return returnObject;
        }
    }
}