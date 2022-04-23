using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.UI
{
    public class RadioButton: IButtonBase
    {
        private string _name;
        private string _text;
        private bool _toggledOn = false;

        private Vector2 _pos;
        private DebugOptions _state;
        private Vector2 _textOffset;
        
        public RadioButton(Vector2 pos, DebugOptions buttonState, string name, string text, Vector2 textOffset)
        {
            _pos = pos;
            _state = buttonState;
            _name = name;
            _text = text;
            _textOffset = textOffset;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _text, _pos + _textOffset, Color.Black);
            
            if (_toggledOn)
            {
                spriteBatch.Draw(TextureManager.Instance.GetButtonTexture("SelectedRadioButton"), _pos, Color.White);
            }
            else
            {
                spriteBatch.Draw(TextureManager.Instance.GetButtonTexture("RadioButton"), _pos, Color.White);
            }
        }        
        
        public string GetName()
        {
            return _name;
        }

        public void OnClick(MouseState mouseState)
        {
            bool gotClicked = UiHandler.CheckClick(mouseState, _pos,
                TextureManager.Instance.GetButtonTexture("RadioButton").Width,
                TextureManager.Instance.GetButtonTexture("RadioButton").Height);

            if (_toggledOn && gotClicked || !gotClicked)
            {
                _toggledOn = false;
                DebugManager.Instance.RemoveDebugOption(_state);
            }
            else
            {
                _toggledOn = true;
                DebugManager.Instance.AddDebugOption(_state);
            }
        }
    }
}