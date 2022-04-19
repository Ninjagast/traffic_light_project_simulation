using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.enums;

namespace traffic_light_simulation.classes.UI
{
    public class ButtonBase
    {
        private int _x;
        private int _y;
        private Texture2D _texture;
        public string Name;
        private ButtonStates _state;
        
        public ButtonBase(int x, int y, string name, ButtonStates buttonState ,Texture2D texture2D)
        {
            this._x = x;
            this._y = y;
            Name = name;
            _texture = texture2D;
            _state = buttonState;
        }
        
        // @return true: if the player has clicked on this button
        private bool CheckClick(MouseState mouseState)
        {
            Point point = new Point(mouseState.X, mouseState.Y);

            if (point.X > _x && point.X < _x + _texture.Width)
            {
                if (point.Y > _y && point.Y < _y + _texture.Height)
                {
                    return true;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, new Vector2(_x, _y), Color.White);
        }
        
        public void OnClick(MouseState mouseState)
        {
            if (CheckClick(mouseState))
            {
                Console.WriteLine($"{Name} got clicked");
                UiHandler.Instance.SetButtonState(_state);
            }
        }
    }
}