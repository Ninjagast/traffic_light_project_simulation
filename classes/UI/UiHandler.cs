using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;

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
        
        private Dictionary<string, IButtonBase> _buttons = new Dictionary<string, IButtonBase>();
        private Dictionary<string, IInputField> _inputFields = new Dictionary<string, IInputField>();
        private ButtonStates _currentButtonState = ButtonStates.Nothing;


        public void Subscribe(IButtonBase buttonBase)
        {
            _buttons.Add(buttonBase.GetName(), buttonBase);
        }

        public void Subscribe(IInputField inputField
        )
        {
            _inputFields.Add(inputField.GetName(), inputField);
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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var button in _buttons)
            {
                button.Value.Draw(spriteBatch);
            }

            foreach (var inputField in _inputFields)
            {
                inputField.Value.Draw(spriteBatch);
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
                        
                        UnSubscribe("PlayButton");
                        UnSubscribe("DebugButton");
                        UnSubscribe("SessionNameField");
                        UnSubscribe("SessionVersionField");

                        Server.Instance.StartServer();
                        SimulationStateHandler.Instance.State = SimulationStates.WaitingForConnection;
                    }
                    break;
                    
                case ButtonStates.DebugPlayButton:
                    UnSubscribe("PlayButton");
                    UnSubscribe("ShowClaimedCells");
                    UnSubscribe("ClaimedCells");
                    UnSubscribe("CarIds");
                    UnSubscribe("TrafficLightIds");
                    UnSubscribe("BicycleLightIds");
                    UnSubscribe("PedestrianLightIds");
                    EventManagerEm.Instance.Subscribe(DebugManager.Instance);
                    Server.Instance.StartServer();
                    SimulationStateHandler.Instance.State = SimulationStates.WaitingForConnection;
                    break;
                
                case ButtonStates.DebugButton:
                    if (_inputFields["SessionNameField"].GetUserInput().Length > 0 && _inputFields["SessionVersionField"].GetUserInput().Length > 0)
                    {
                        Server.Instance.SetServerNameVersion(_inputFields["SessionNameField"].GetUserInput(), _inputFields["SessionVersionField"].GetUserInput());

                        UnSubscribe("PlayButton");
                        UnSubscribe("DebugButton");
                        UnSubscribe("SessionNameField");
                        UnSubscribe("SessionVersionField");

                        CreationManager.CreateDebugButtons();
                        SimulationStateHandler.Instance.State = SimulationStates.SettingUpDebugMode;
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
    }
}