using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace traffic_light_simulation.classes.UI
{
    public interface IButtonBase
    {
        public void Draw(SpriteBatch spriteBatch);
        public void OnClick(MouseState mouseState);
        public string GetName();
    }
}