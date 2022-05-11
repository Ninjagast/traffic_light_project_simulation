using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class HitTree: IDrawAble
    {
        private int _currentFrame;
        private Vector2 _pos;
        private Dictionary<string, Vector2> _targetAreas;
        private States _state = States.Open;
        private Dictionary<String, int> _stoppedEntities = new Dictionary<string, int>();
        private string _direction;
        private int _id = 0;
        private string _offset;
        
        public void Update()
        {
            if (_targetAreas.Count > 0)
            {
                if (_state == States.Closed && _stoppedEntities.Count < 3)
                {
                    int carId =  VehicleEm.Instance.GetCellCarId(_targetAreas["car"]);
                    int bikeId = VehicleEm.Instance.GetCellPeopleId(_targetAreas["notCar"], _direction);
                    int guyId =  VehicleEm.Instance.GetBikeId(_targetAreas["notCar"], _direction);

                    if (carId < 0 && bikeId < 0 && guyId < 0)
                    {
                        return;
                    }
                    if(carId > -1 && !_stoppedEntities.ContainsKey("car"))
                    {
                        _stoppedEntities.Add("car", carId);
                        VehicleEm.Instance.OnStateChange(carId, States.Idle);
                    }
                    if(bikeId > -1 && !_stoppedEntities.ContainsKey("bike"))
                    {
                        _stoppedEntities.Add("bike", bikeId);
                        VehicleEm.Instance.OnStateChange(bikeId, States.Idle);
                    }
                    if(guyId > -1 && !_stoppedEntities.ContainsKey("guy"))
                    {
                        _stoppedEntities.Add("guy", guyId);
                        VehicleEm.Instance.OnStateChange(guyId, States.Idle);
                    }
                }
                else if (_state == States.Open)
                {
                    if (_stoppedEntities.Count > 0)
                    {
                        foreach (var id in _stoppedEntities)
                        {
                            VehicleEm.Instance.OnStateChange(id.Value, States.Transit);
                        }
                        _stoppedEntities.Clear();
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_state == States.Open)
            {
                spriteBatch.Draw(TextureManager.Instance.GetTexture("HitTreeOpen"), _pos, Color.White);
            }
            else
            {
                Vector2 offset = Vector2.Zero;
                switch (_offset)
                {
                    case "RIGHT":
                        offset = new Vector2(50, 0); // not used in the current simulation
                        break;
                    case "LEFT":
                        offset = Vector2.Zero; // not used in the current simulation
                        break;
                    case "DOWN":
                        offset = Vector2.Zero; //verified
                        break;
                    case "UP":
                        offset = new Vector2(-100, -70); //verified
                        break;
                }
                spriteBatch.Draw(TextureManager.Instance.GetTexture("HitTreeClosed"), new Vector2(_pos.X, _pos.Y + TextureManager.Instance.GetTexture("HitTreeOpen").Height) + offset, Color.White);
            }
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
            spriteBatch.DrawString(TextureManager.Instance.GetFont(),$"{_id}", _pos, Color.Black);
        }

        public void DrawTargetArea(SpriteBatch spriteBatch)
        {
            if (_targetAreas.Count > 0)
            {
                spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                    new Rectangle((int) _targetAreas["car"].X - 22, (int) _targetAreas["car"].Y + 12, 99, 50), Color.Lime);
                
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
                    new Rectangle((int) ((_targetAreas["notCar"].X - 25) + offset.X), (int) ((_targetAreas["notCar"].Y + 15) + offset.Y), 50, 25), Color.Lime);
            }   
        }

        public static HitTree CreateInstance(Vector2 pos, Dictionary<string, Vector2> targetAreas, string direction, string offset)
        {
            return new HitTree{_pos = pos, _targetAreas = targetAreas, _direction = direction, _offset = offset};
        }
    }
}