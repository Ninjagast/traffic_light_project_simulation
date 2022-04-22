using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.UI
{
    public class BaseButton: IButtonBase
    {
        private ButtonStates _state;
        private string _name;
        private Vector2 _pos;

        public BaseButton(Vector2 pos, ButtonStates buttonState, string name)
        {
            _pos = pos;
            _state = buttonState;
            _name = name;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureManager.Instance.GetButtonTexture(_name), _pos, Color.White);
        }
        
        public void OnClick(MouseState mouseState)
        {
            if (UiHandler.CheckClick(mouseState, _pos, TextureManager.Instance.GetButtonTexture(_name).Width, TextureManager.Instance.GetButtonTexture(_name).Height))
            {
                UiHandler.Instance.SetButtonState(_state);
            }
        }

        public string GetName()
        {
            return _name;
        }
    }
}