using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Dictionary<Vector2, int> _peopleClaimedCells = new Dictionary<Vector2, int>();
        private Dictionary<Vector2, int> _bikeClaimedCells = new Dictionary<Vector2, int>();
        
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
        public void UnClaimPeopleCell(Vector2 id)
        {
            _peopleClaimedCells.Remove(id);
        }
        public void UnClaimBikeCell(Vector2 id)
        {
            _bikeClaimedCells.Remove(id);
        }
        public void ClaimCell(Vector2 targetPos, int id)
        {
            _claimedCells.Add(targetPos, id);
        }
        public void ClaimPeopleCell(Vector2 targetPos, int id)
        {
            _peopleClaimedCells.Add(targetPos, id);
        }

        public void ClaimBikeCell(Vector2 targetPos, int id)
        {
            _bikeClaimedCells.Add(targetPos, id);
        }
        public bool IsCellFree(Vector2 targetPos)
        {
            return !_claimedCells.ContainsKey(targetPos);
        }
        public bool IsPeopleCellFree(Vector2 targetPos)
        {
            return !_peopleClaimedCells.ContainsKey(targetPos);
        }

        public bool IsBikeCellFree(Vector2 targetPos)
        {
            return !_bikeClaimedCells.ContainsKey(targetPos);
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
                    new Rectangle((int) claimedCell.Key.X - 22, (int) claimedCell.Key.Y + 12, 99, 50), Color.Red);
            }
        }
        
        public void DebugDrawPeopleMarkers(SpriteBatch spriteBatch)
        {
            foreach (var claimedCell in _peopleClaimedCells)
            {
                spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                    new Rectangle((int) claimedCell.Key.X - 22, (int) claimedCell.Key.Y + 12, 99, 50), Color.Cyan);
            }
        }
        
        public void DebugDrawBikeMarkers(SpriteBatch spriteBatch)
        {
            foreach (var claimedCell in _bikeClaimedCells)
            {
                spriteBatch.Draw(TextureManager.Instance.GetDebugTexture("ClaimMarker"),
                    new Rectangle((int) claimedCell.Key.X - 22, (int) claimedCell.Key.Y + 12, 99, 50), Color.Purple);
            }
        }


        public void DebugDrawIds(SpriteBatch spriteBatch)
        {
            foreach (var vehicle in _subscribed)
            {
                vehicle.Value.DrawId(spriteBatch);
            }
        }

        public int GetCellPeopleId(Vector2 targetArea)
        {
            foreach (var cell in _peopleClaimedCells)
            {
                if (cell.Key == targetArea)
                {
                    return cell.Value;
                }
            }

            return -1;
        }

        public int GetBikeId(Vector2 targetArea)
        {
            foreach (var cell in _bikeClaimedCells)
            {
                if (cell.Key == targetArea)
                {
                    return cell.Value;
                }
            }

            return -1;
        }
    }
}