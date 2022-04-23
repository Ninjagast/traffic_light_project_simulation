using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace traffic_light_simulation.classes.UI
{
    public interface IInputField
    {
        public void Draw(SpriteBatch spriteBatch);
        public void OnClick(MouseState mouseState);
        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState);
        public string GetUserInput();
        public string GetName();
    }
}