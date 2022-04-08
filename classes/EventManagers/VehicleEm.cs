using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.EventManagers
{
    public class VehicleEm: IEventManager
    {
        private static VehicleEm _instance;
        private List<IDrawAble> _subscribed = new List<IDrawAble>();
        private static readonly object Padlock = new object();
        private Dictionary<int, Vector2> _claimedCells = new Dictionary<int, Vector2>();

        private int _id = 0;

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
        
        public void Subscribe(IDrawAble drawAble)
        {
            _subscribed.Add(drawAble);
        }

        public void OnStateChange(int id, States state)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.StateChange(id, state);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            foreach (var subbed in _subscribed)
            {
                subbed.Update();
            }
        }

        public int GetNextId()
        {
            _id += 1;
            return _id;
        }

        public void UnClaimCell(int id)
        {
            _claimedCells.Remove(id);
        }
        public void ClaimCell(Vector2 targetPos, int id)
        {
            _claimedCells.Add(id, targetPos);
        }

        public bool IsCellFree(Vector2 targetPos)
        {
            return !_claimedCells.ContainsValue(targetPos);
        }

        public void UnSubscribe(int id)
        {
            _subscribed.RemoveAt(id);
        }

        public int GetCellCarId(Vector2 targetArea)
        {
            foreach (var cell in _claimedCells)
            {
                if (cell.Value == targetArea)
                {
                    return cell.Key;
                }
            }

            return -1;
        }
    }
}