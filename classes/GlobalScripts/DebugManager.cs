using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class DebugManager: IEventManager
    {
        private static DebugManager _instance;
        private static readonly object Padlock = new object();
        private DebugManager() {}
        public static DebugManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DebugManager();
                    }
                    return _instance;
                }
            }
        }
        public bool DrawClaimedCells { get; set; } = false;
        
        
        public void Subscribe(IDrawAble drawAble)
        {
            throw new System.NotImplementedException();
        }

        public void OnStateChange(int id, States state)
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (DrawClaimedCells)
            {
                foreach (var VARIABLE in VehicleEm.Instance.GetCellGrid())
                {
                    spriteBatch.Draw(TextureManager.Instance.GetTexture(2, "BikeGreen"),
                        new Rectangle((int) VARIABLE.Key.X, (int) VARIABLE.Key.Y, 20, 50), Color.White);
                }
            }
        }

        public void Update()
        {
            return;
        }
    }
}