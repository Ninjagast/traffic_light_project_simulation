using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.UI
{
    public class RadioButtonGroup
    {
        public string Name { get; set; }
        
        private List<IButtonBase> _radioButtons = new List<IButtonBase>();
        private Vector2 _pos;
        private Vector2 _offset;
        private Vector2 _textOffset;

        public RadioButtonGroup(Vector2 pos, string name, Dictionary<string, DebugOptions> radioButtons, List<string> texts ,Vector2 offset, Vector2 textOffset)
        {
            _offset = offset;
            _pos = pos;
            Name = name;
            _textOffset = textOffset;
            _createRadioButtons(radioButtons, texts);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var radioButton in _radioButtons)
            {
                radioButton.Draw(spriteBatch);
            }
        }

        private void _createRadioButtons(Dictionary<string, DebugOptions> radioButtons, List<string> texts)
        {
            int vectorMultiplier = 0;
            foreach (var radioButton in radioButtons)
            {
                IButtonBase newButton = new RadioButton((_pos + (_offset * vectorMultiplier)), radioButton.Value, radioButton.Key, texts[vectorMultiplier], _textOffset);
                _radioButtons.Add(newButton);
                vectorMultiplier++;
            }
        }

        public void OnClick(MouseState mouseState)
        {
            int clickedIndex = -1;
            for (int i = 0; i < _radioButtons.Count; i++)
            {
                if (UiHandler.CheckClick(mouseState, (_pos + (_offset * i)),
                    TextureManager.Instance.GetButtonTexture("RadioButton").Width,
                    TextureManager.Instance.GetButtonTexture("RadioButton").Height))
                {
                    clickedIndex = i;
                }
            }

            if (clickedIndex != -1)
            {
                foreach (var radioButton in _radioButtons)
                {
                    radioButton.OnClick(mouseState);
                }
            }
        }
    }
}