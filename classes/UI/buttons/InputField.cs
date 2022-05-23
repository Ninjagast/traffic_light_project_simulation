using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.GlobalScripts;


namespace traffic_light_simulation.classes.UI.buttons
{
    public class InputField: IInputField
    {
        private bool _selected = false;
        private string _userInput = "";
        private string _name;
        
        private Vector2 _textLocation;
        private Vector2 _pos;

        public InputField(Vector2 pos, string name)
        {
            _pos = pos;
            _textLocation = _pos + new Vector2(10, 25);
            _name = name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_selected)
            {
                spriteBatch.Draw(TextureManager.Instance.GetButtonTexture("FieldSelectedTexture"), _pos, Color.White);
            }
            else
            {
                spriteBatch.Draw(TextureManager.Instance.GetButtonTexture("FieldTexture"), _pos, Color.White);
            }
            spriteBatch.DrawString(TextureManager.Instance.GetFont(), _userInput, _textLocation, Color.Black);
        }

        public void OnClick(MouseState mouseState)
        {
            _selected = UiHandler.CheckClick(mouseState, _pos, TextureManager.Instance.GetButtonTexture("FieldTexture").Width, TextureManager.Instance.GetButtonTexture("FieldTexture").Height);
        }

        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            if (_selected)
            {
                _handleKeyboardPressesFields(keyboardState, prevKeyboardState);
            }
        }

        public string GetUserInput()
        {
            return _userInput;
        }

        public string GetName()
        {
            return _name;
        }

//      ##################################################
//      Helper functions
        private void _handleKeyboardPressesFields(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            foreach (var pressedKeyId in keyboardState.GetPressedKeys())
            {
                if (prevKeyboardState.IsKeyUp(pressedKeyId))
                {
                    if(CheckIfLetterOrKey((int)pressedKeyId))
                    {
                        if (_userInput.Length < 10)
                        {
                            string letter = pressedKeyId.ToString().Length > 1 ? pressedKeyId.ToString()[1].ToString(): pressedKeyId.ToString();
                            letter = letter.ToLower();

                            if (keyboardState.CapsLock)
                            {
                                letter = letter.ToUpper();
                            }
                            else if (keyboardState.IsKeyDown(Keys.LeftShift))
                            {
                                letter = letter.ToUpper();
                            }
                            else if(keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                letter = letter.ToUpper();
                            }

                            _userInput += letter;
                        }
                    }
                    else if (pressedKeyId == Keys.Back)
                    {
//                      if this is the keyPressDown frame && it is the backspace && we have selected the sessionNameField 
                        if (_userInput.Length > 0)
                        {
                            _userInput = _userInput.Remove(_userInput.Length - 1, 1);  
                        }
                    }
                }   
            }
        }

        private bool CheckIfLetterOrKey(int key)
        {
            if (key > 64 && key < 91 || key > 47 && key < 58)
            {
                return true;
            }

            return false;
        }
    }
}