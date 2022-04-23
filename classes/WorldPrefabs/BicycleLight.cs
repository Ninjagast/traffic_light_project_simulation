using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class BicycleLight : IDrawAble
    {
        
        private int _laneId;
        private Vector2 _pos;
        private States _state; 

        public void Update()
        {
            return;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetTexture(2, "Bike" + _state.ToString()), new Rectangle((int) _pos.X, (int)_pos.Y, 20, 50), Color.White);
        }

        public void StateChange(int id, States state)
        {
            if (_laneId == id)
            {
                _state = state;
            }
        }

        public void DrawId(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _laneId.ToString(), new Vector2(_pos.X, _pos.Y - 10), Color.Black);
        }

        public static BicycleLight CreateInstance(Vector2 pos, int routeId)
        {
            BicycleLight returnInstance = new BicycleLight
            {
                _laneId = routeId, _pos = pos, _state = States.Red
            };
            return returnInstance;
        }
    }
}