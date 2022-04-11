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
        private directionMap _directionMap;
        private string _lastDirection;
        private int _currentFrame = 0;
        private int _id = -1;
        private int _step = 0;
        private int _repetition = 0;
        private Dictionary<string, Vector2> _orientation;
        private bool _claimedStandingCell = false;
        private SpriteFont _font;
        
        public void Update()
        {
            if (_state == States.Idle)
            {
                if (!_claimedStandingCell)
                {
                    VehicleEm.Instance.ClaimCell(_pos, _id);
                    _claimedStandingCell = true;
                }
            }
            else if(_state == States.Transit)
            {
                _currentFrame += 1;
                _pos += _orientation[_lastDirection];
                if (_currentFrame == 50)
                {
                    _claimedStandingCell = false;
                    _state = States.Driving;
                    Console.WriteLine("current position");
                    Console.WriteLine(_pos);
                    VehicleEm.Instance.UnClaimCell(_pos);
                    _currentFrame = 0;
                }
            }
            else if (_state == States.Driving)
            {
                Vector2 targetPos = _pos + (_orientation[_lastDirection] * (50 - _currentFrame));
                if ((_step >= _directionMap.directions.directions.Count) || (_directionMap.directions.directions.Count == 1 && _repetition == _directionMap.directions.repeat_first))
                {
                    VehicleEm.Instance.UnClaimCell(_pos);
                    VehicleEm.Instance.UnSubscribe(_id); //todo might create a memory leak
                    return;
                }
                
                if (VehicleEm.Instance.IsCellFree(targetPos))
                {
                    _claimedStandingCell = false;
                    VehicleEm.Instance.UnClaimCell(_pos);
                    _state = States.Transit;
                    if (_step == 0)
                    {
                        if (_repetition < _directionMap.directions.repeat_first)
                        {                            
                            VehicleEm.Instance.ClaimCell(targetPos, _id);        
                            _lastDirection = _directionMap.directions.directions[0];
                            _repetition += 1;
                        }
                        else
                        {
                            _lastDirection = _directionMap.directions.directions[0];
                            VehicleEm.Instance.ClaimCell(targetPos, _id);        
                            _step += 1;
                        }
                    }
                    else
                    {
                        _lastDirection = _directionMap.directions.directions[_step];
                        Vector2 newTargetPos = _pos + (_orientation[_lastDirection] * (50 - _currentFrame));
                        VehicleEm.Instance.ClaimCell(newTargetPos, _id);        
                        _step += 1;
                    }
                }
                else
                {
                    if (!_claimedStandingCell)
                    {
                        VehicleEm.Instance.ClaimCell(_pos, _id);
                        _claimedStandingCell = true;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(0, "sedan_" + _lastDirection), new Rectangle((int)_pos.X, (int)_pos.Y, 50, 50), Color.White);
            spriteBatch.DrawString(_font, _id.ToString(), _pos,Color.Black);
        }

        public void StateChange(int id, States state)
        {
            if (_id == id)
            {
                if (state == States.Transit)
                {
                    VehicleEm.Instance.UnClaimCell(_pos);
                }
                _state = state;
            }
        }

        public static Car CreateInstance(SpriteFont font, bool testing = true)
        {
            Car returnObject = new Car();
            if (testing)
            {
                returnObject._directionMap = SpawnPoints.Instance.GetFromTestRoutes();
                returnObject._pos = new Vector2(returnObject._directionMap.vector2.x, returnObject._directionMap.vector2.y);
            }
            else
            {
                returnObject._directionMap = SpawnPoints.Instance.GetRandomLandSpawnPoint();
                returnObject._pos = new Vector2(returnObject._directionMap.vector2.x, returnObject._directionMap.vector2.y);
            }
            returnObject._state = States.Driving;
            returnObject._currentFrame = 0;
            returnObject._orientation = new Dictionary<string, Vector2>();
            returnObject._orientation.Add("LEFT", new Vector2(-1, 0.5f));
            returnObject._orientation.Add("RIGHT", new Vector2(1,-0.5f));
            returnObject._orientation.Add("DOWN", new Vector2(1, 0.5f));
            returnObject._orientation.Add("UP", new Vector2(-1, -0.5f));
            returnObject._lastDirection = returnObject._directionMap.directions.directions[0];
            returnObject._id = VehicleEm.Instance.GetNextId();
            returnObject._font = font;
            return returnObject;
        }
    }
}