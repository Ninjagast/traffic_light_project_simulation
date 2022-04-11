using System.Collections.Generic;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.EventManagers;
using traffic_light_simulation.classes.GlobalScripts;
using traffic_light_simulation.classes.WorldPrefabs;

namespace traffic_light_simulation
{
    public class World : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private float _updateHrtz = 60f;
        private Camera _camera;

        private SpriteFont _font;
        private Texture2D _backGround;
        
        private List<string> _orientations;
        private List<string> _ligtStates;

        private bool _paused = true;

        private int tick = 0;

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

            _ligtStates = new List<string>
            {
                "Green", "Orange", "Red"
            };
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Server.Instance.StartServer();
            SpawnPoints.Instance.GetSpawnPoints();
            base.Initialize();
            _graphics.PreferredBackBufferHeight = 750;
            _graphics.PreferredBackBufferWidth = 750;
            _camera = new Camera(_graphics.GraphicsDevice.Viewport);
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
//          Loading the textures
            _backGround = Content.Load<Texture2D>("BackGround");
            _font = Content.Load<SpriteFont>("File");

            Dictionary<string, Texture2D> sedanTextures = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> trafficLightTextures = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> pedestrianLightTextures = new Dictionary<string, Texture2D>();
            Dictionary<string, Texture2D> bicycleLightTextures = new Dictionary<string, Texture2D>();

            foreach (var orientation in _orientations)
            {
                 sedanTextures.Add($"sedan_{orientation}", Content.Load<Texture2D>($"sedan_{orientation}"));
            }
            foreach (var color in _ligtStates)
            {
                trafficLightTextures.Add($"Light{color}", Content.Load<Texture2D>($"Light{color}"));
                bicycleLightTextures.Add($"Bike{color}", Content.Load<Texture2D>($"Bike{color}"));
            }
            
            pedestrianLightTextures.Add("PeopleGreen", Content.Load<Texture2D>("PeopleGreen"));
            pedestrianLightTextures.Add("PeopleRed", Content.Load<Texture2D>("PeopleRed"));

            TextureManager.Instance.SetTexture(sedanTextures, 0);
            TextureManager.Instance.SetTexture(trafficLightTextures, 1);
            TextureManager.Instance.SetTexture(bicycleLightTextures, 2);
            TextureManager.Instance.SetTexture(pedestrianLightTextures, 3);
            
            //Left crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(380,520), 9, _font, new Vector2(375, 520)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(430,550), 8, _font, new Vector2(425, 545)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(480,570), 8, _font, new Vector2(475, 570)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(530,590), 7, _font, new Vector2(525, 595)));
            
            //Bottom crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1400,500), 4, _font, new Vector2(1380, 520)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1350,525), 4, _font, new Vector2(1320, 540)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1300,550), 5, _font, new Vector2(1275, 570)));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(590,700), 15, _font));
            
            //Right crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(800,101), 1, _font , new Vector2(775,115)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(850,130), 2, _font , new Vector2(825, 140)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(900,160), 2, _font , new Vector2(875, 165)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(950,180), 3, _font , new Vector2(925, 190)));
            
            //Top crosspoint
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(290,250), 10, _font, new Vector2(275, 270)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(340,230), 11, _font, new Vector2(325, 245)));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(390,210), 12, _font, new Vector2(375, 220)));

            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,200), 31, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,600), 32, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,650), 33, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,650), 34, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,600), 35, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,200), 36, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,100), 37, _font));
            // PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,100), 38, _font));
            
            // BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(550, 350), 21, _font));            
            // BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 550), 22, _font));            
            // BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(150, 350), 23, _font));            
            // BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 150), 24, _font));            

            VehicleEm.Instance.Subscribe(Car.CreateInstance(_font));
        }

        protected override void Update(GameTime gameTime)
        {
            if (!_paused)
            {
                tick += 1;
                if (tick == 200)
                {
                    VehicleEm.Instance.Subscribe(Car.CreateInstance(_font));
                    tick = 0;
                }
                EventManagerEm.Instance.Update();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _paused = true;            
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                _paused = false;
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // if (!Server.Instance.HasConnection)
            // {
                // return;
            // }
            
            _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // if (!Server.Instance.HasConnection)
            // {
                // return;
            // }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
            _spriteBatch.Draw(_backGround, new Rectangle(0,0,1850,815), Color.White);
            EventManagerEm.Instance.Draw(_spriteBatch);
            if (_paused)
            {
                _spriteBatch.DrawString(_font, "The game is paused jackass", new Vector2(50,50), Color.White);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
