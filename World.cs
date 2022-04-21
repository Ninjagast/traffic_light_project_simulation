using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
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

        private int _tick = 0;
        
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
            
            _prevKeyboardState = Keyboard.GetState();
            _prevMouseState    = Mouse.GetState();
            
            _graphics.ApplyChanges();
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
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("DebugButton"), "DebugButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("FieldTexture"), "FieldTexture");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("PlayButton"), "PlayButton");
            
            CreationManager.CreateTrafficLights();
            CreationManager.CreateStartScreenButtons();
            // CreationManager.CreatePedestrianLights();
            // CreationManager.CreateBicycleLights();
            // CreationManager.CreateBoatLights();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            SimulationStates currentState = SimulationStateHandler.Instance.State;
            
            if (currentState == SimulationStates.Running)
            {
                CheckKeyPress(Keys.Space,SimulationStates.Paused);
                
                _tick += 1;
                if (_tick % 20 == 0)
                {
                    Car car = Car.CreateInstance(VehicleEm.Instance.Testing, _random);
                    if (car != null)
                    {
                        VehicleEm.Instance.Subscribe(car);
                    }
                }
                _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                EventManagerEm.Instance.Update();
            }
            else if (currentState == SimulationStates.Paused)
            {
                _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                CheckKeyPress(Keys.Space, SimulationStates.Running);
            }
            else if (currentState == SimulationStates.StartScreen)
            {
                CheckMousePress();
            }
            else if (currentState == SimulationStates.SettingUpDebugMode)
            {
                CheckMousePress();
            }
            else if (currentState == SimulationStates.WaitingForConnection)
            {
                if (Server.Instance.HasConnection)
                {
                    SimulationStateHandler.Instance.State = SimulationStates.Running;
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
            SimulationStates currentState = SimulationStateHandler.Instance.State;

            if (currentState == SimulationStates.Running)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_backGround, new Rectangle(0,0,1850,815), Color.White);
                EventManagerEm.Instance.Draw(_spriteBatch);
            }
            else if (currentState == SimulationStates.Paused)
            {
                GraphicsDevice.Clear(Color.Gray);
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_backGround, new Rectangle(0,0,1850,815), Color.White);
                EventManagerEm.Instance.Draw(_spriteBatch);
            }
            else if (currentState == SimulationStates.StartScreen)
            {
                _spriteBatch.Begin();
                UiHandler.Instance.Draw(_spriteBatch);
            }
            else if (currentState == SimulationStates.SettingUpDebugMode)
            {
                _spriteBatch.Begin();
                UiHandler.Instance.Draw(_spriteBatch);
            }
            else// _state == WaitingForConnection.
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(TextureManager.Instance.getFont() ,"Waiting for a connection", new Vector2(250,250), Color.Black);
            }
            
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        
//      #######################################################
//      Helper functions
        private void CheckMousePress()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _prevMouseState.LeftButton != ButtonState.Pressed)
            {
                UiHandler.Instance.CheckClick(Mouse.GetState());
            }
        }

        private void CheckKeyPress(Keys key, SimulationStates state)
        {
            if (Keyboard.GetState().IsKeyDown(key) && _prevKeyboardState.IsKeyUp(key))
            {
                SimulationStateHandler.Instance.State = state;
            }
        }
    }
}
