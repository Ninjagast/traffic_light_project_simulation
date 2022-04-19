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
        private Dictionary<Vector2, int> _claimedCells = new Dictionary<Vector2, int>();
        public bool Testing = true;
        
//      Three speed modifiers possible 1 / 2 / 5
        public int Speed = 1;


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
            _subscribed.Add(_id, drawAble);
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

            foreach (var VARIABLE in _claimedCells)
            {
                spriteBatch.Draw(TextureManager.Instance.GetTexture(2, "BikeGreen"),
                    new Rectangle((int) VARIABLE.Key.X, (int) VARIABLE.Key.Y, 20, 50), Color.White);
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
        public void ClaimCell(Vector2 targetPos, int id)
        {
            _claimedCells.Add(targetPos, id);
        }

        public bool IsCellFree(Vector2 targetPos)
        {
            return !_claimedCells.ContainsKey(targetPos);
        }

        public void UnSubscribe(int id)
        {
            _subscribed.Remove(id);
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
    }
}