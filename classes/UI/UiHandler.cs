using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation.classes.UI
{
    public class UiHandler
    {
        private static UiHandler _instance;
        private Dictionary<string, ButtonBase> _buttons = new Dictionary<string, ButtonBase>();
        private static readonly object Padlock = new object();
        private ButtonStates _currentButtonState = ButtonStates.Nothing;
        private string _sessionName = "";
        private string _sessionVersion = "";
        private World _world;

        private UiHandler() {}
        public static UiHandler Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new UiHandler();
                    }
                    return _instance;
                }
            }
        }

        public void SetWorld(World world)
        {
            _world = world;
        }
        
        public void Subscribe(ButtonBase button)
        {
            _buttons.Add(button.Name, button);
        }

        public void UnSubscribe(string name)
        {
            _buttons.Remove(name);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Value.Draw(spriteBatch);
            }
            
            spriteBatch.DrawString(TextureManager.Instance.getFont(), _sessionName, new Vector2(310, 125), Color.Black);
            spriteBatch.DrawString(TextureManager.Instance.getFont(), _sessionVersion, new Vector2(310, 175), Color.Black);
        }

        public void CheckClick(MouseState mouseState)
        {
            foreach (var button in _buttons)
            {
                button.Value.OnClick(mouseState);
            }
        }

        public void SetButtonState(ButtonStates state)
        {
            _currentButtonState = state;
        }

        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            switch (_currentButtonState)
            {
                case ButtonStates.DebugButton:
                    UnSubscribe("PlayButton");
                    UnSubscribe("DebugButton");
                    UnSubscribe("FieldTexture");
                    UnSubscribe("FieldTexture");

                    CreationManager.CreateDebugButtons();
                    _world.SetState(SimulationStates.SettingUpDebugMode);
                    break;
                
                case ButtonStates.PlayButton:
                    if (_sessionName.Length > 0 && _sessionVersion.Length > 0)
                    {
                        UnSubscribe("PlayButton");
                        UnSubscribe("DebugButton");
                        UnSubscribe("FieldTexture");
                        UnSubscribe("FieldTexture");

                        Server.Instance.StartServer(_sessionName, _sessionVersion);
                        _world.SetState(SimulationStates.WaitingForConnection);
                    }
                    break;
                    
                case ButtonStates.SessionNameField:
                    _handleKeyboardPressesFields(keyboardState, prevKeyboardState, true);
                    return;
                
                case ButtonStates.SessionVersionField:
                    _handleKeyboardPressesFields(keyboardState, prevKeyboardState, false);
                    return;
                
                case ButtonStates.Nothing:
                    return;
                
                default:
                    Console.WriteLine($"{_currentButtonState} buttonState does not exist");
                    break;

            }
            _currentButtonState = ButtonStates.Nothing;
        }
        
//      ##################################################
//      Helper functions
        private void _handleKeyboardPressesFields(KeyboardState keyboardState, KeyboardState prevKeyboardState, bool sessionName)
        {
            foreach (var pressedKeyId in keyboardState.GetPressedKeys())
            {
                if (prevKeyboardState.IsKeyUp(pressedKeyId))
                {
                    if(CheckIfLetterOrKey((int)pressedKeyId))
                    {
                        if (sessionName)
                        {
//                          if this is the keyPressDown frame && it is a letter or a number && we have selected the sessionNameField 
                            if (_sessionName.Length < 10)
                            {
                                string letter = pressedKeyId.ToString().Length > 1 ? pressedKeyId.ToString()[1].ToString(): pressedKeyId.ToString();
                                if (!keyboardState.CapsLock || !keyboardState.IsKeyDown(Keys.LeftShift) ||
                                    !keyboardState.IsKeyDown(Keys.RightShift))
                                {
                                    letter = letter.ToLower();
                                }

                                _sessionName += letter;
                            }
                        }
                        else
                        {
//                          if this is the keyPressDown frame && it is a letter or a number && we have selected the sessionVersionField 
                            if(_sessionVersion.Length < 10)
                            {
                                string letter = pressedKeyId.ToString().Length > 1 ? pressedKeyId.ToString()[1].ToString(): pressedKeyId.ToString();
                                if (!keyboardState.CapsLock || !keyboardState.IsKeyDown(Keys.LeftShift) ||
                                    !keyboardState.IsKeyDown(Keys.RightShift))
                                {
                                    letter = letter.ToLower();
                                }
                                _sessionVersion += letter;
                            }
                        }
                    }
                    else if (pressedKeyId == Keys.Back)
                    {
                        if (sessionName)
                        {
//                          if this is the keyPressDown frame && it is the backspace && we have selected the sessionNameField 
                            if (_sessionName.Length > 0)
                            {
                                _sessionName = _sessionName.Remove(_sessionName.Length - 1, 1);  
                            }
                        }
                        else
                        {
//                          if this is the keyPressDown frame && it is the backspace && we have selected the sessionVersionField 
                            if (_sessionVersion.Length > 0)
                            {
                                _sessionVersion = _sessionVersion.Remove(_sessionVersion.Length - 1, 1);  
                            }
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