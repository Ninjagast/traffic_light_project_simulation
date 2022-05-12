using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.dataClasses;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;
using traffic_light_simulation.classes.WorldPrefabs;


namespace traffic_light_simulation.classes.EventManagers
{
    public class VehicleEm: IEventManager
    {
        private static VehicleEm _instance;
        private Dictionary<int, IDrawAble> _subscribed = new Dictionary<int, IDrawAble>();
        private static readonly object Padlock = new object();
        private VehicleEm() {}
        public static VehicleEm Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new VehicleEm();
                    }
                    return _instance;
                }
            }
        }
        
//      Three speed modifiers possible 0.5 / 1 / 2 / 5
        public int DefaultSpeed = 2;
        private int _id = 0;

        private Dictionary<Vector2, int> _claimedCells = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, DivCell> _peopleClaimedCells = new Dictionary<Vector2, DivCell>();
        private Dictionary<Vector2, DivCell> _bikeClaimedCells = new Dictionary<Vector2, DivCell>();
        private Dictionary<Vector2, DivCell> _boatClaimedCells = new Dictionary<Vector2, DivCell>();
        private List<Vector2> _exceptions = new List<Vector2> {new Vector2(1025, 995), new Vector2(575, 870), new Vector2(825, 1195), new Vector2(475, 1170)};
        
        public void Subscribe(IDrawAble drawAble)
        {
            _subscribed.Add(_id, drawAble);
        }

        public void UnSubscribe(int id)
        {
            _subscribed.Remove(id);
        }
        
        public void OnStateChange(int id, States state)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Value.StateChange(id, state);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Value.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Value.Update();
            }
        }

        public int GetNextId()
        {
            _id += 1;
            return _id;
        }

        public void UnClaimCell(Vector2 id)
        {
            _claimedCells.Remove(id);
        }
        
        public void UnClaimPeopleCell(Vector2 id, string direction)
        {
            _peopleClaimedCells[id].UnClaimCell(direction);
        }
        
        public void UnClaimBikeCell(Vector2 id, string direction)
        {
            _bikeClaimedCells[id].UnClaimCell(direction);
        }
        
        public void UnClaimBoatCell(Vector2 pos, string direction)
        {
            _boatClaimedCells[pos].UnClaimCell(direction);
        }
        
        public void ClaimCell(Vector2 targetPos, int id)
        {
            _claimedCells.Add(targetPos, id);
        }
        
        public void ClaimPeopleCell(Vector2 targetPos, int id, string direction)
        {
            if (_peopleClaimedCells.ContainsKey(targetPos))
            {
                _peopleClaimedCells[targetPos].ClaimCell(id, direction);
            }
            else
            {
                DivCell newCellDiv = new DivCell();
                newCellDiv.ClaimCell(id, direction);
                _peopleClaimedCells.Add(targetPos, newCellDiv);
            }
        }

        public void ClaimBoatCell(Vector2 targetPos, int id, string direction)
        {
            if (_boatClaimedCells.ContainsKey(targetPos))
            {
                _boatClaimedCells[targetPos].ClaimCell(id, direction);
            }
            else
            {
                DivCell newCellDiv = new DivCell();
                newCellDiv.ClaimCell(id, direction);
                _boatClaimedCells.Add(targetPos, newCellDiv);
            }
        }
        
        public void ClaimBikeCell(Vector2 targetPos, int id, string direction)
        {
            if (_bikeClaimedCells.ContainsKey(targetPos))
            {
                _bikeClaimedCells[targetPos].ClaimCell(id, direction);
            }
            else
            {
                DivCell newCellDiv = new DivCell();
                newCellDiv.ClaimCell(id, direction);
                _bikeClaimedCells.Add(targetPos, newCellDiv);
            }
        }
        
        public bool IsCellFree(Vector2 targetPos)
        {
            if (_exceptions.Contains(targetPos))
            {
                return true;
            }
            if (_claimedCells.ContainsKey(targetPos))
            {
                return false;
            }

            if (_bikeClaimedCells.ContainsKey(targetPos))
            {
                if (!_bikeClaimedCells[targetPos].IsCellFree())
                {
                    return false;
                }
            }
            
            if (_peopleClaimedCells.ContainsKey(targetPos))
            {
                if (!_peopleClaimedCells[targetPos].IsCellFree())
                {
                    return false;
                }
            }

            return true;
        }
        
        public bool IsPeopleCellFree(Vector2 targetPos, string direction)
        {
            if (_peopleClaimedCells.ContainsKey(targetPos))
            {
                return _peopleClaimedCells[targetPos].IsCellFree(direction);
            }
            else
            {
                return true;
            }
        }

        public bool IsBikeCellFree(Vector2 targetPos, string direction)
        {
            if (_bikeClaimedCells.ContainsKey(targetPos))
            {
                return _bikeClaimedCells[targetPos].IsCellFree(direction);
            }
            else
            {
                return true;
            }
        }

        public bool IsBoatCellFree(Vector2 targetPos, string direction)
        {
            if (_boatClaimedCells.ContainsKey(targetPos))
            {
                return _boatClaimedCells[targetPos].IsCellFree(direction);
            }
            else
            {
                return true;
            }
        }
        
        public int GetCellCarId(Vector2 targetArea)
        {
            foreach (var cell in _claimedCells)
            {
                if (cell.Key == targetArea)
                {
                    return cell.Value;
                }
            }

            return -1;
        }

        public void DebugDrawCarMarkers(SpriteBatch spriteBatch)
        {
            foreach (var claimedCell in _claimedCells)
            {
                spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                    new Rectangle((int) claimedCell.Key.X - 22, (int) claimedCell.Key.Y + 12, 100, 50), Color.Red);
            }
        }
        
        public void DebugDrawPeopleMarkers(SpriteBatch spriteBatch)
        {
            foreach (var claimedCell in _peopleClaimedCells)
            {
                List<Vector2> claimedCells = claimedCell.Value.GetClaimedCells();
                if (claimedCells.Count > 0)
                {
                    foreach (var offset in claimedCells)
                    {
                        spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                            new Rectangle((int) ((claimedCell.Key.X - 25) + offset.X), (int) ((claimedCell.Key.Y + 15) + offset.Y), 50, 25), Color.Black);
                    }
                }
            }
        }
        
        public void DebugDrawBikeMarkers(SpriteBatch spriteBatch)
        {
            foreach (var claimedCell in _bikeClaimedCells)
            {
                List<Vector2> claimedCells = claimedCell.Value.GetClaimedCells();
                if (claimedCells.Count > 0)
                {
                    foreach (var offset in claimedCells)
                    {
                        spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                            new Rectangle((int) ((claimedCell.Key.X - 25) + offset.X), (int) ((claimedCell.Key.Y + 15) + offset.Y), 50, 25), Color.Purple);
                    }
                }
            }
        }

        public void DebugDrawIds(SpriteBatch spriteBatch)
        {
            foreach (var vehicle in _subscribed)
            {
                vehicle.Value.DrawId(spriteBatch);
            }
        }

        public int GetCellPeopleId(Vector2 targetArea, string direction)
        {
            if (_peopleClaimedCells.ContainsKey(targetArea))
            {
                return _peopleClaimedCells[targetArea].GetCellId(direction);
            }
            else
            {
                return -1;
            }
        }
        public int GetCellBoatId(Vector2 targetArea, string direction)
        {
            if (_boatClaimedCells.ContainsKey(targetArea))
            {
                return _boatClaimedCells[targetArea].GetCellId(direction);
            }
            else
            {
                return -1;
            }
        }
        
        public int GetBikeId(Vector2 targetArea, string direction)
        {
            if (_bikeClaimedCells.ContainsKey(targetArea))
            {
                return _bikeClaimedCells[targetArea].GetCellId(direction);
            }
            else
            {
                return -1;
            }
        }
        
    }
}