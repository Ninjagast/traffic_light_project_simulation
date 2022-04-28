using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.debug;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;
using traffic_light_simulation.classes.UI;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation
{
    public class World : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _backGround;
        
        private List<string> _orientations;
        private List<string> _lightStates;

        private Random _random;
        private Camera _camera;
        
        private MouseState _prevMouseState;
        private KeyboardState _prevKeyboardState;
        

        public World()
        {
            EventManagerEm.Instance.Subscribe(TrafficLightEm.Instance);
            EventManagerEm.Instance.Subscribe(PedestrianLightEm.Instance);
            EventManagerEm.Instance.Subscribe(BicycleLightEm.Instance);
            EventManagerEm.Instance.Subscribe(VehicleEm.Instance);

            _orientations = new List<string>
            {
                "UP", "DOWN", "LEFT", "RIGHT"
            };

            _lightStates = new List<string>
            {
                "Green", "Orange", "Red"
            };
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            SpawnPoints.Instance.GetSpawnPoints();
            
            _graphics.PreferredBackBufferHeight = 750;
            _graphics.PreferredBackBufferWidth  = 750;
            
            _camera = new Camera(_graphics.GraphicsDevice.Viewport);
            _random = new Random();
            WeightTableHandler.Instance.CreateTables(_random);
            
            _prevKeyboardState = Keyboard.GetState();
            _prevMouseState    = Mouse.GetState();
            
            _graphics.ApplyChanges();
            
            Window.AllowAltF4 = false; //Alt+F4 is not allowed
            Exiting += _closingStatements;
            base.Initialize();
        }
        
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
//          Loading the textures
            _backGround = Content.Load<Texture2D>("BackGround");

            Dictionary<string, Texture2D> sedanTextures           = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> trafficLightTextures    = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> pedestrianLightTextures = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> bicycleLightTextures    = new Dictionary<string, Texture2D>();

            foreach (var orientation in _orientations)
            {
                 sedanTextures.Add($"sedan_{orientation}", Content.Load<Texture2D>($"sedan_{orientation}"));
            }
            foreach (var color in _lightStates)
            {
                trafficLightTextures.Add($"Light{color}", Content.Load<Texture2D>($"Light{color}"));
                bicycleLightTextures.Add($"Bike{color}",  Content.Load<Texture2D>($"Bike{color}"));
            }
            
            pedestrianLightTextures.Add("PeopleGreen", Content.Load<Texture2D>("PeopleGreen"));
            pedestrianLightTextures.Add("PeopleRed",   Content.Load<Texture2D>("PeopleRed"));

            TextureManager.Instance.SetFont(Content.Load<SpriteFont>("File"));
            TextureManager.Instance.SetTexture(sedanTextures, 0);
            TextureManager.Instance.SetTexture(trafficLightTextures, 1);
            TextureManager.Instance.SetTexture(bicycleLightTextures, 2);
            TextureManager.Instance.SetTexture(pedestrianLightTextures, 3);
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("FieldTexture"), "FieldTexture");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("FieldSelectedTexture"), "FieldSelectedTexture");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("DebugButton"), "DebugButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("PlayButton"), "PlayButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("CheckedCheckBox"), "CheckedCheckBox");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("CheckBox"), "CheckBox");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("SelectedRadioButton"), "SelectedRadioButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("ReplayButton"), "ReplayButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("RadioButton"), "RadioButton");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("ClaimMarker"), "ClaimMarker");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("PeopleGreen"), "People");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("BikeGreen"), "Bike");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("fietser"), "fietser");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("mensen"), "mensen");
            
            CreationManager.CreateTrafficLights();
            CreationManager.CreateStartScreenButtons();
            CreationManager.CreateBicycleLights();
            CreationManager.CreatePedestrianLights();
            // CreationManager.CreatePedestrianLights();
            // CreationManager.CreateBicycleLights();
            // CreationManager.CreateBoatLights();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            SimulationStates currentState = EventManagerEm.Instance.State;
            
            if (currentState == SimulationStates.Running)
            {
                DebugManager.Instance.UpdateTick += 1;
                _checkKeyPress(Keys.Space,SimulationStates.Paused);

                if (_random.Next(0, 400) > 398) // 0.25% chance per tick to spawn a random Guy
                {
                    People people = People.CreateInstance(_random);
                    if (people != null)
                    {
                        VehicleEm.Instance.Subscribe(people);
                    }
                }
                if (_random.Next(0, 50) > 48) // 2% chance per tick to spawn a random car
                {
                    Car car = Car.CreateInstance(_random);
                    if (car != null)
                    {
                        VehicleEm.Instance.Subscribe(car);
                    }
                }
                if (_random.Next(0, 400) > 398) // 0.25% chance per tick to spawn a random Bike
                {
                    Bike bike = Bike.CreateInstance(_random);
                    if (bike != null)
                    {
                        VehicleEm.Instance.Subscribe(bike);
                    }
                }
                _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                EventManagerEm.Instance.Update();
            }
            else if (currentState == SimulationStates.Paused)
            {
                _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                _checkKeyPress(Keys.Space, SimulationStates.Running);
            }
            else if (currentState == SimulationStates.PausedReplay)
            {
                _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                _checkKeyPress(Keys.Space, SimulationStates.Replaying);
            }
            else if (currentState == SimulationStates.Replaying)
            {
                _checkKeyPress(Keys.Space,SimulationStates.PausedReplay);

                DebugManager.Instance.UpdateTick += 1;
                ReplayManager.Instance.CheckTick();
                                
                _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                EventManagerEm.Instance.Update();
            }
            else if (currentState == SimulationStates.StartScreen)
            {
                _checkMousePress();
            }
            else if (currentState == SimulationStates.SettingUpDebugMode)
            {
                _checkMousePress();
            }
            else if (currentState == SimulationStates.WaitingForConnection)
            {
                if (Server.Instance.HasConnection)
                {
                    EventManagerEm.Instance.State = SimulationStates.Running;
                }
            }

            UiHandler.Instance.Update(Keyboard.GetState(), _prevKeyboardState);
            _prevKeyboardState = Keyboard.GetState();
            _prevMouseState = Mouse.GetState();
            base.Update(gameTime);   
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SimulationStates currentState = EventManagerEm.Instance.State;

            if (currentState == SimulationStates.Running || currentState == SimulationStates.Replaying)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_backGround, new Rectangle(0,0,2945,1491), Color.White);
                UiHandler.Instance.Draw(_spriteBatch, _camera.Pos + new Vector2(-400, -400));
                EventManagerEm.Instance.Draw(_spriteBatch);
            }
            else if (currentState == SimulationStates.Paused || currentState == SimulationStates.PausedReplay)
            {
                GraphicsDevice.Clear(Color.Gray);
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_backGround, new Rectangle(0,0,2945,1491), Color.White);
                EventManagerEm.Instance.Draw(_spriteBatch);
            }
            else if (currentState == SimulationStates.StartScreen)
            {
                _spriteBatch.Begin();
                UiHandler.Instance.Draw(_spriteBatch, Vector2.One);
            }
            else if (currentState == SimulationStates.SettingUpDebugMode)
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(TextureManager.Instance.GetFont(), "", new Vector2(5,5), Color.Black);
                UiHandler.Instance.Draw(_spriteBatch, Vector2.One);
            }
            else// _state == WaitingForConnection.
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(TextureManager.Instance.GetFont() ,"Waiting for a connection", new Vector2(250,250), Color.Black);
            }
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
        private void _closingStatements(object? sender, EventArgs eventArgs)
        {
            Logger.Instance.CreateLogs();
        }
        
//      #######################################################
//      Helper functions
        private void _checkMousePress()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton != ButtonState.Pressed)
            {
                UiHandler.Instance.CheckClick(Mouse.GetState());
            }
        }

        private void _checkKeyPress(Keys key, SimulationStates state)
        {
            if (Keyboard.GetState().IsKeyDown(key) && _prevKeyboardState.IsKeyUp(key))
            {
                EventManagerEm.Instance.State = state;
            }
        }
    }
}
