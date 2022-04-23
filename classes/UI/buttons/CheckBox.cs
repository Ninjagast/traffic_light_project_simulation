using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.UI.buttons
{
    public class CheckBox : IButtonBase
    {
        private string _name;
        private string _text;
        private bool _toggledOn = false;

        private Vector2 _pos;
        private Vector2 _textOffset;
        private DebugOptions _option;

        public CheckBox(Vector2 pos, string text, DebugOptions option, Vector2 textOffset, string name)
        {
            _pos = pos;
            _text = text;
            _option = option;
            _textOffset = textOffset;
            _name = name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _text, _pos + _textOffset, Color.Black);

            if (_toggledOn)
            {
                spriteBatch.Draw(TextureManager.Instance.GetButtonTexture("CheckedCheckBox"), _pos, Color.White);
            }
            else
            {
                spriteBatch.Draw(TextureManager.Instance.GetButtonTexture("CheckBox"), _pos, Color.White);
            }
        }

        public void OnClick(MouseState mouseState)
        {
            if (UiHandler.CheckClick(mouseState, _pos, TextureManager.Instance.GetButtonTexture("CheckBox").Width,
                TextureManager.Instance.GetButtonTexture("CheckBox").Height))
            {
                _toggledOn = !_toggledOn;
                if (_toggledOn)
                {
                    DebugManager.Instance.AddDebugOption(_option);
                }
                else
                {
                    DebugManager.Instance.RemoveDebugOption(_option);
                }
            }
            
        }

        public string GetName()
        {
            return _name;
        }
    }
}