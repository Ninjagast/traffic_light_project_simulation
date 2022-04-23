using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;
using traffic_light_simulation.classes.UI.buttons;


namespace traffic_light_simulation.classes.UI
{
    public class UiHandler
    {
        private static UiHandler _instance;
        private static readonly object Padlock = new object();
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
        
        private Dictionary<string, IButtonBase>      _buttons           = new Dictionary<string, IButtonBase>();
        private Dictionary<string, IInputField>      _inputFields       = new Dictionary<string, IInputField>();
        private Dictionary<string, RadioButtonGroup> _radioButtonGroups = new Dictionary<string, RadioButtonGroup>();
        private List<string>                         _warnings          = new List<string>();
        private float _warningCounter = 300f;
        
        private ButtonStates _currentButtonState = ButtonStates.Nothing;

        public void Subscribe(IButtonBase buttonBase)
        {
            _buttons.Add(buttonBase.GetName(), buttonBase);
        }

        public void Subscribe(IInputField inputField)
        {
            _inputFields.Add(inputField.GetName(), inputField);
        }

        public void Subscribe(RadioButtonGroup radioButtonGroup)
        {
            _radioButtonGroups.Add(radioButtonGroup.Name, radioButtonGroup);
        }

        public void UnSubscribe(string name)
        {
            if (_buttons.ContainsKey(name))
            {
                _buttons.Remove(name);
            }
            else
            {
                _inputFields.Remove(name);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 offsetPosition)
        {
            foreach (var button in _buttons)
            {
                button.Value.Draw(spriteBatch);
            }

            foreach (var inputField in _inputFields)
            {
                inputField.Value.Draw(spriteBatch);
            }
            
            foreach (var radioButton in _radioButtonGroups)
            {
                radioButton.Value.Draw(spriteBatch);
            }

            if (_warnings.Count > 0)
            {
                _warningCounter -= 1;
                if (_warningCounter < 2)
                {
                    _warningCounter = 300f;
                    _warnings.RemoveAt(0);
                }
                else
                {
                    spriteBatch.DrawString(TextureManager.Instance.GetFont(), _warnings[0], new Vector2(10, 10) + offsetPosition, Color.Red * (_warningCounter / 300));
                }
            
            }
        }

        public void CheckClick(MouseState mouseState)
        {
            foreach (var button in _buttons)
            {
                button.Value.OnClick(mouseState);
            }

            foreach (var inputField in _inputFields)
            {
                inputField.Value.OnClick(mouseState);
            }
            
            foreach (var radioButtonGroup in _radioButtonGroups)
            {
                radioButtonGroup.Value.OnClick(mouseState);
            }
        }

        public void SetButtonState(ButtonStates state)
        {
            _currentButtonState = state;
        }

        public void Update(KeyboardState keyboardState, KeyboardState prevKeyboardState)
        {
            foreach (var inputField in _inputFields)
            {
                inputField.Value.Update(keyboardState, prevKeyboardState);
            }

            switch (_currentButtonState)
            {
//              ##################################################################################################
//              Start screen buttons 
                case ButtonStates.PlayButton:
                    if (_inputFields["SessionNameField"].GetUserInput().Length > 0 && _inputFields["SessionVersionField"].GetUserInput().Length > 0)
                    {
                        Server.Instance.SetServerNameVersion(_inputFields["SessionNameField"].GetUserInput(), _inputFields["SessionVersionField"].GetUserInput());
                        
                        _massUnsub();
                        Server.Instance.StartServer();
                        EventManagerEm.Instance.State = SimulationStates.WaitingForConnection;
                    }
                    break;
                    
                case ButtonStates.DebugPlayButton:
                    
                    _massUnsub();
                    EventManagerEm.Instance.Subscribe(DebugManager.Instance);
                    DebugManager.Instance.SetUp();
                    Server.Instance.StartServer();
                    EventManagerEm.Instance.State = SimulationStates.WaitingForConnection;
                    break;
                
                case ButtonStates.DebugButton:
                    if (_inputFields["SessionNameField"].GetUserInput().Length > 0 && _inputFields["SessionVersionField"].GetUserInput().Length > 0)
                    {
                        Server.Instance.SetServerNameVersion(_inputFields["SessionNameField"].GetUserInput(), _inputFields["SessionVersionField"].GetUserInput());

                        _massUnsub();
                        CreationManager.CreateDebugButtons();
                        EventManagerEm.Instance.State = SimulationStates.SettingUpDebugMode;
                    }
                    break;
                
                case ButtonStates.Replay:
                    Logger.Instance.SetUp();
                    if (Logger.Instance.DoesALogExist())
                    {
                        if (ReplayManager.Instance.ProcessLogs())
                        {
                            _massUnsub();
                            EventManagerEm.Instance.State = SimulationStates.Replaying;
                        }
                        else
                        {
                            _warnings.Add("The logs are invalid");
                        }
                    }
                    else
                    {
                        _warnings.Add("No available logs exist (check names)");
                    }
                    break;
                
                case ButtonStates.Nothing:
                    return;
                
                default:
                    Console.WriteLine($"{_currentButtonState} buttonState does not exist");
                    break;

            }
            _currentButtonState = ButtonStates.Nothing;
        }

        private void _massUnsub()
        {
            _inputFields = new Dictionary<string, IInputField>();
            _buttons = new Dictionary<string, IButtonBase>();
            _radioButtonGroups = new Dictionary<string, RadioButtonGroup>();
            _warnings = new List<string>();
            _warningCounter = 300f;
        }
        
        public static bool CheckClick(MouseState mouseState, Vector2 pos, int textureWidth, int textureHeight)
        {
            Point point = new Point(mouseState.X, mouseState.Y);
            if (point.X > pos.X && point.X < pos.X + textureWidth)
            {
                if (point.Y > pos.Y && point.Y < pos.Y + textureHeight)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddWarning(string warning)
        {
            _warnings.Add(warning);
        }
    }
}