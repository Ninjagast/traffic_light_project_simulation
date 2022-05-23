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
//          subscribe all EventManagers
            EventManagerEm.Instance.Subscribe(BridgeEm.Instance);
            EventManagerEm.Instance.Subscribe(TrafficLightEm.Instance);
            EventManagerEm.Instance.Subscribe(PedestrianLightEm.Instance);
            EventManagerEm.Instance.Subscribe(BicycleLightEm.Instance);
            EventManagerEm.Instance.Subscribe(VehicleEm.Instance);
            EventManagerEm.Instance.Subscribe(BridgeHitTreeEm.Instance);
            EventManagerEm.Instance.Subscribe(BridgeWarningLightEm.Instance);

//          orientations match the name of the variations in perspective on the graphics
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

//      initialize content set Game configs
        protected override void Initialize()
        {
//          loads in the spawn points from the routeFiles
            SpawnPoints.Instance.GetSpawnPoints();
            
//          sets window size (we don't have a scalable window/dynamic positions)
            _graphics.PreferredBackBufferHeight = 750;
            _graphics.PreferredBackBufferWidth  = 750;
            
            _camera = new Camera(_graphics.GraphicsDevice.Viewport);
            _random = new Random();
            
//          creates the Weighted Probability tables for the spawn points
            WeightTableHandler.Instance.CreateTables(_random);
            
            _prevKeyboardState = Keyboard.GetState();
            _prevMouseState    = Mouse.GetState();
            
            _graphics.ApplyChanges();
            
            Window.AllowAltF4 = false;
            
//          subscribes the closingstatements function to the exiting event
            Exiting += _closingStatements;
            base.Initialize();
        }
        
//      load game content (graphics/textures/fonts)
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
//          Loading the textures
            _backGround = Content.Load<Texture2D>("BackGround");
            
//          prefab textures
            Dictionary<string, Texture2D> sedanTextures           = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> taxiTextures            = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> hatchBackTextures       = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> personATextures         = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> smallBoatTextures       = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> trafficLightTextures    = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> pedestrianLightTextures = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> bicycleLightTextures    = new Dictionary<string, Texture2D>();

            foreach (var orientation in _orientations)
            {
                 sedanTextures.Add($"sedan_{orientation}", Content.Load<Texture2D>($"sedan_{orientation}"));
                 personATextures.Add($"personA_{orientation}", Content.Load<Texture2D>($"personA_{orientation}"));
                 smallBoatTextures.Add($"boat_small_{orientation}", Content.Load<Texture2D>($"boat_small_{orientation}"));
                 taxiTextures.Add($"taxi_{orientation}", Content.Load<Texture2D>($"taxi_{orientation}"));
                 hatchBackTextures.Add($"hatchBack_{orientation}", Content.Load<Texture2D>($"hatchBack_{orientation}"));
            }
            foreach (var color in _lightStates)
            {
                trafficLightTextures.Add($"Light{color}", Content.Load<Texture2D>($"Light{color}"));
                bicycleLightTextures.Add($"Bike{color}",  Content.Load<Texture2D>($"Bike{color}"));
            }
            
            pedestrianLightTextures.Add("PeopleGreen", Content.Load<Texture2D>("PeopleGreen"));
            pedestrianLightTextures.Add("PeopleRed",   Content.Load<Texture2D>("PeopleRed"));
            
            TextureManager.Instance.SetTexture(sedanTextures);
            TextureManager.Instance.SetTexture(trafficLightTextures);
            TextureManager.Instance.SetTexture(bicycleLightTextures);
            TextureManager.Instance.SetTexture(pedestrianLightTextures);
            TextureManager.Instance.SetTexture(personATextures);
            TextureManager.Instance.SetTexture(smallBoatTextures);
            TextureManager.Instance.SetTexture(taxiTextures);
            TextureManager.Instance.SetTexture(hatchBackTextures);
            TextureManager.Instance.SetTexture(Content.Load<Texture2D>("bridgeClosed"), "bridgeClosed");
            TextureManager.Instance.SetTexture(Content.Load<Texture2D>("bridgeOpen"), "bridgeOpen");
            TextureManager.Instance.SetTexture(Content.Load<Texture2D>("HitTreeOpen"), "HitTreeOpen");
            TextureManager.Instance.SetTexture(Content.Load<Texture2D>("HitTreeClosed"), "HitTreeClosed");
            
//          Fonts
            TextureManager.Instance.AddFont(Content.Load<SpriteFont>("SmallFont"), "SmallFont");
            TextureManager.Instance.AddFont(Content.Load<SpriteFont>("BigFont"), "BigFont");

//          Button textures
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("FieldTexture"), "FieldTexture");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("FieldSelectedTexture"), "FieldSelectedTexture");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("DebugButton"), "DebugButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("PlayButton"), "PlayButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("CheckedCheckBox"), "CheckedCheckBox");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("CheckBox"), "CheckBox");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("SelectedRadioButton"), "SelectedRadioButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("ReplayButton"), "ReplayButton");
            TextureManager.Instance.AddButtonTexture(Content.Load<Texture2D>("RadioButton"), "RadioButton");
            
//          Debug Textures
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("ClaimMarker"), "ClaimMarker");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("PeopleGreen"), "People");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("BikeGreen"), "Bike");
            TextureManager.Instance.AddDebugTexture(Content.Load<Texture2D>("fietser"), "fietser");
            
//          Content push all the static prefabs
            CreationManager.CreateTrafficLights();
            CreationManager.CreateStartScreenButtons();
            CreationManager.CreateBicycleLights();
            CreationManager.CreatePedestrianLights();
            CreationManager.CreateBridges();
            CreationManager.CreateHitTrees();
        }

//      update function gets called once per frame
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

//          do stuff based on current simulation state
            switch (EventManagerEm.Instance.State)
            {
                case SimulationStates.Running:
                    DebugManager.Instance.UpdateTick += 1;
                    _checkKeyPress(Keys.Space,SimulationStates.Paused);
                    _randomSpawn();
                    _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                    EventManagerEm.Instance.Update();
                    break;
                
                case SimulationStates.Paused:
                    _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                    _checkKeyPress(Keys.Space, SimulationStates.Running);
                    break;
                
                case SimulationStates.PausedReplay:
                    _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                    _checkKeyPress(Keys.Space, SimulationStates.Replaying);
                    break;
                
                case SimulationStates.Replaying:
                    _checkKeyPress(Keys.Space,SimulationStates.PausedReplay);

                    DebugManager.Instance.UpdateTick += 1;
                    ReplayManager.Instance.CheckTick();
                
                    _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
                    EventManagerEm.Instance.Update();
                    break;
                
                case SimulationStates.StartScreen:
                    _checkMousePress();
                    break;                
                
                case SimulationStates.SettingUpDebugMode:
                    _checkMousePress();
                    break;
                
                case SimulationStates.WaitingForConnection:
                    if (Server.Instance.HasConnection)
                    {
                        EventManagerEm.Instance.State = SimulationStates.Running;
                    }
                    break;
            }

            UiHandler.Instance.Update(Keyboard.GetState(), _prevKeyboardState);
            _prevKeyboardState = Keyboard.GetState();
            _prevMouseState = Mouse.GetState();
            base.Update(gameTime);   
        }
        
//      gets called once per frame after the update function
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (EventManagerEm.Instance.State == SimulationStates.Running || EventManagerEm.Instance.State == SimulationStates.Replaying)
            {
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_backGround, new Rectangle(0,0,2945,1491), Color.White);
                UiHandler.Instance.Draw(_spriteBatch, _camera.GetPos());
                EventManagerEm.Instance.Draw(_spriteBatch);
            }
            else if (EventManagerEm.Instance.State == SimulationStates.Paused || EventManagerEm.Instance.State == SimulationStates.PausedReplay)
            {
                GraphicsDevice.Clear(Color.Gray);
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
                _spriteBatch.Draw(_backGround, new Rectangle(0,0,2945,1491), Color.White);
                EventManagerEm.Instance.Draw(_spriteBatch);
            }
            else if (EventManagerEm.Instance.State == SimulationStates.StartScreen)
            {
                _spriteBatch.Begin();
                UiHandler.Instance.Draw(_spriteBatch, Vector2.One);
            }
            else if (EventManagerEm.Instance.State == SimulationStates.SettingUpDebugMode)
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

        private void _randomSpawn()
        {
            if (_random.Next(0, 600) > 598) // 0.16% chance per tick to spawn a random Guy
            {
                People people = People.CreateInstance(_random);
                if (people != null)
                {
                    VehicleEm.Instance.Subscribe(people);
                }
            }
            if (_random.Next(0, 200) > 198) // 0.5% chance per tick to spawn a random car
            {
                Car car = Car.CreateInstance(_random);
                if (car != null)
                {
                    VehicleEm.Instance.Subscribe(car);
                }
            }
            if (_random.Next(0, 600) > 598) // 0.16% chance per tick to spawn a random Bike
            {
                Bike bike = Bike.CreateInstance(_random);
                if (bike != null)
                {
                    VehicleEm.Instance.Subscribe(bike);
                }
            }
            if (_random.Next(0, 2400) > 2398) // 0.04% chance per tick to spawn a random Boat
            {
                Boat boat = Boat.CreateInstance(_random);
                if (boat != null)
                {
                    VehicleEm.Instance.Subscribe(boat);
                }
            }
        }
    }
}
