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
        private Dictionary<string, Texture2D> _textures;
        private Vector2 _pos;
        private States _state;
        private directionMap _directionMap;
        private string _lastDirection;
        private Vector2 TargetPos;
        private int _currentFrame = 0;
        private int _id = -1;
        private int _step = 0;
        private int _repetition = 0;
        private Dictionary<string, Vector2> _orientation;
        
        public void Update()
        {
            if (_state == States.IDLE)
            {
                return;
            }
            
            if(_state == States.TRANSIT)
            {
                _currentFrame += 1;
                _pos += _orientation[_lastDirection];
                if (_currentFrame > 50)
                {
                    _state = States.DRIVING;
                    Console.WriteLine(_pos);
                    _currentFrame = 0;
                    VehicleEm.Instance.ClaimCell(_pos, _id);        
                }

            }
            else if (_state == States.DRIVING)
            {
                Vector2 targetPos = _pos + (_orientation[_lastDirection] * (60 - _currentFrame));
//              todo some weirdness going on
                if (_step - 1 >= _directionMap.directions.directions.Count)
                {
                    VehicleEm.Instance.UnClaimCell(_id);
                    VehicleEm.Instance.UnSubscribe(_id);
                    return;
                }
                
                if (VehicleEm.Instance.IsCellFree(targetPos))
                {
                    VehicleEm.Instance.UnClaimCell(_id);
                    _state = States.TRANSIT;
                    if (_step == 0)
                    {
                        if (_repetition < _directionMap.directions.repeat_first)
                        {
                            _lastDirection = _directionMap.directions.directions[0];
                            _repetition += 1;
                        }
                        else
                        {
                            _lastDirection = _directionMap.directions.directions[0];
                            if (_directionMap.directions.directions[1] == "LEFT")
                            {
                                _currentFrame = -15;
                            }
                            else
                            {
                                _currentFrame = 15;
                            }
                            _step += 1;
                        }
                    }
                    else
                    {
                        _lastDirection = _directionMap.directions.directions[_step];
                        _step += 1;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
//          todo need to change the size based on texture also add rotation to the draw parameter based on direction
            switch (_lastDirection)
            {
                case "UP":
                    spriteBatch.Draw(_textures["sedan_SW"], new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
                    break;
                
                case "DOWN":
                    spriteBatch.Draw(_textures["sedan_NE"], new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
                    break;
                
                case "LEFT":
                    spriteBatch.Draw(_textures["sedan_SE"], new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
                    break;
                
                case "RIGHT":
                    spriteBatch.Draw(_textures["sedan_NW"], new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
                    break;
                
                default:
                    break;
                    
            }
        }

        public void StateChange(int id, States state)
        {
            _state = state;
        }

        public static Car CreateInstance(Dictionary<string, Texture2D> texture2D)
        {
            Car returnObject = new Car();
            SpawnPoints.Instance.GetSpawnPoints();
            returnObject._directionMap = SpawnPoints.Instance.GetRandomLandSpawnPoint();
            returnObject._pos = new Vector2(returnObject._directionMap.vector2.x, returnObject._directionMap.vector2.y);
            returnObject._textures = texture2D;
            returnObject._state = States.DRIVING;
            returnObject._currentFrame = 0;
            returnObject._orientation = new Dictionary<string, Vector2>();
            returnObject._orientation.Add("LEFT", new Vector2(-1, 0.5f));
            returnObject._orientation.Add("RIGHT", new Vector2(1,-0.5f));
            returnObject._orientation.Add("DOWN", new Vector2(1, 0.5f));
            returnObject._orientation.Add("UP", new Vector2(-1, -0.5f));
            returnObject._lastDirection = returnObject._directionMap.directions.directions[0];
            returnObject._id = VehicleEm.Instance.GetNextId();
            return returnObject;
        }
    }
}