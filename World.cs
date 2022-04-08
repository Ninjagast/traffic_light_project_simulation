using System.Collections.Generic;
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
        private float _updateHrtz;
        private Camera _camera;

        private Texture2D _lightGreen;
        private Texture2D _lightOrange;
        private Texture2D _lightRed;
        private Texture2D _peopleRed;
        private Texture2D _peopleGreen;
        private Texture2D _lightBikeGreen;
        private Texture2D _lightBikeOrange;
        private Texture2D _lightBikeRed;
        private Texture2D _backGround;
        
        private SpriteFont _font;

        private List<string> _orientations;
        private Dictionary<string, Texture2D> _sedanTextures;

        public World()
        {
            EventManagerEm.Instance.Subscribe(TrafficLightEm.Instance);
            EventManagerEm.Instance.Subscribe(PedestrianLightEm.Instance);
            EventManagerEm.Instance.Subscribe(BicycleLightEm.Instance);
            EventManagerEm.Instance.Subscribe(VehicleEm.Instance);

            _orientations = new List<string>
            {
                "E", "N", "NE", "NW", "S", "SE", "SW", "W"
            };

            _sedanTextures = new Dictionary<string, Texture2D>();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Server.Instance.StartServer();
            base.Initialize();
            _graphics.PreferredBackBufferHeight = 750;
            _graphics.PreferredBackBufferWidth = 750;
            _camera = new Camera(_graphics.GraphicsDevice.Viewport);
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _lightGreen = Content.Load<Texture2D>("LightGreen");
            _lightRed = Content.Load<Texture2D>("LightRed");
            _lightOrange = Content.Load<Texture2D>("LightYellow");            
            _lightBikeGreen = Content.Load<Texture2D>("BikeGreen");
            _lightBikeRed = Content.Load<Texture2D>("BikeRed");
            _lightBikeOrange = Content.Load<Texture2D>("BikeOrange");
            _peopleGreen = Content.Load<Texture2D>("PeopleGreen");
            _peopleRed = Content.Load<Texture2D>("PeopleRed");
            _font = Content.Load<SpriteFont>("File");
            _backGround = Content.Load<Texture2D>("BackGround (1)");

            foreach (var orientation in _orientations)
            {
                _sedanTextures.Add($"sedan_{orientation}", Content.Load<Texture2D>($"sedan_{orientation}"));
            }
            
            // TODO: use this.Content to load your game content here
            
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(800,101), 1, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(850,130), 2, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(900,160), 2, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(950,180), 3, _lightRed, _lightOrange, _lightGreen, _font));

            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(380,520), 9, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(430,550), 8, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(480,570), 8, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(530,590), 7, _lightRed, _lightOrange, _lightGreen, _font));

            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1400,500), 4, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1350,525), 4, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(1300,550), 5, _lightRed, _lightOrange, _lightGreen, _font));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(590,700), 15, _lightRed, _lightOrange, _lightGreen, _font));
            
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(290,250), 10, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(340,230), 11, _lightRed, _lightOrange, _lightGreen, _font, new Vector2(280, 222.5f)));
            // TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(390,210), 12, _lightRed, _lightOrange, _lightGreen, _font));
            
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,200), 31, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,600), 32, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,650), 33, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,650), 34, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,600), 35, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,200), 36, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,100), 37, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,100), 38, _peopleRed, _peopleGreen, _font));
            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(550, 350), 21, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 550), 22, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(150, 350), 23, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 150), 24, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            

            VehicleEm.Instance.Subscribe(Car.CreateInstance(_sedanTextures));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (!Server.Instance.HasConnection)
            {
                return;
            }
            
            _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
            EventManagerEm.Instance.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!Server.Instance.HasConnection)
            {
                return;
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
            _spriteBatch.Draw(_backGround, new Rectangle(0,0,1850,815), Color.White);
            EventManagerEm.Instance.Draw(_spriteBatch);
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
