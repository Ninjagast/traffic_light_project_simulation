using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class HitTree: IDrawAble
    {
        private Vector2 _pos;
        private States _state = States.Open;
        private int _id = 0;
        private string _offset;
        
        public void Update()
        {
            return;
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
            return;
        }

        public static HitTree CreateInstance(Vector2 pos, string offset, int bridgeId)
        {
            return new HitTree{_pos = pos, _offset = offset, _id = bridgeId};
        }
    }
}