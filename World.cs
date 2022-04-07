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
        private Texture2D _dirt;
        private Server _server;
        private Texture2D _gridOutline;
        private Camera _camera;

        private Texture2D _lightGreen;
        private Texture2D _lightOrange;
        private Texture2D _lightRed;
        private Texture2D _peopleRed;
        private Texture2D _peopleGreen;
        private Texture2D _lightBikeGreen;
        private Texture2D _lightBikeOrange;
        private Texture2D _lightBikeRed;
        
        private SpriteFont _font;

        public World()
        {
            EventManagerEm.Instance.Subscribe(TrafficLightEm.Instance);
            EventManagerEm.Instance.Subscribe(PedestrianLightEm.Instance);
            EventManagerEm.Instance.Subscribe(BicycleLightEm.Instance);

            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _server = new Server();
            _server.StartServer();
            base.Initialize();
            _graphics.PreferredBackBufferHeight = 750;
            _graphics.PreferredBackBufferWidth = 750;
            _camera = new Camera(_graphics.GraphicsDevice.Viewport);
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _dirt = Content.Load<Texture2D>("dirt");
            _gridOutline = Content.Load<Texture2D>("gridTile");
            _lightGreen = Content.Load<Texture2D>("LightGreen");
            _lightRed = Content.Load<Texture2D>("LightRed");
            _lightOrange = Content.Load<Texture2D>("LightYellow");            
            _lightBikeGreen = Content.Load<Texture2D>("BikeGreen");
            _lightBikeRed = Content.Load<Texture2D>("BikeRed");
            _lightBikeOrange = Content.Load<Texture2D>("BikeOrange");
            _peopleGreen = Content.Load<Texture2D>("PeopleGreen");
            _peopleRed = Content.Load<Texture2D>("PeopleRed");
            _font = Content.Load<SpriteFont>("File");
            // TODO: use this.Content to load your game content here
            
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,100), 1, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,130), 2, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,160), 2, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,190), 3, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(250,10), 10, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(270,10), 11, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(290,10), 12, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,400), 7, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,430), 8, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,460), 8, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,490), 9, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(500,700), 5, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(530,700), 4, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(560,700), 4, _lightRed, _lightOrange, _lightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(590,700), 15, _lightRed, _lightOrange, _lightGreen, _font));

            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,650), 34, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,650), 33, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,100), 37, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,100), 38, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,200), 36, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,600), 35, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,200), 31, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,600), 32, _peopleRed, _peopleGreen, _font));
            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 150), 24, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(150, 350), 23, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 550), 22, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(550, 350), 21, _lightBikeRed, _lightBikeOrange,_lightBikeGreen, _font));            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);
            EventManagerEm.Instance.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
            
            EventManagerEm.Instance.Draw(_spriteBatch);
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
