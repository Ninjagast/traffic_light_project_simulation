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

        private Texture2D _LightGreen;
        private Texture2D _LightOrange;
        private Texture2D _LightRed;
        private Texture2D _peopleRed;
        private Texture2D _peopleGreen;
        private SpriteFont _font;
        private Texture2D _LightBikeGreen;
        private Texture2D _LightBikeOrange;
        private Texture2D _LightBikeRed;

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
            _LightGreen = Content.Load<Texture2D>("LightGreen");
            _LightRed = Content.Load<Texture2D>("LightRed");
            _LightOrange = Content.Load<Texture2D>("LightYellow");            
            _LightBikeGreen = Content.Load<Texture2D>("BikeGreen");
            _LightBikeRed = Content.Load<Texture2D>("BikeRed");
            _LightBikeOrange = Content.Load<Texture2D>("BikeOrange");
            _peopleGreen = Content.Load<Texture2D>("PeopleGreen");
            _peopleRed = Content.Load<Texture2D>("PeopleRed");
            _font = Content.Load<SpriteFont>("File");
            // TODO: use this.Content to load your game content here
            
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,100), 1, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,130), 2, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,160), 2, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(710,190), 3, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(250,10), 10, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(270,10), 11, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(290,10), 12, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,400), 7, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,430), 8, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,460), 8, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(20,490), 9, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(500,700), 5, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(530,700), 4, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(560,700), 4, _LightRed, _LightOrange, _LightGreen, _font));
            TrafficLightEm.Instance.Subscribe(TrafficLight.CreateInstance(new Vector2(590,700), 15, _LightRed, _LightOrange, _LightGreen, _font));

            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,650), 34, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,650), 33, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(250,100), 37, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(500,100), 38, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,200), 36, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(100,600), 35, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,200), 31, _peopleRed, _peopleGreen, _font));
            PedestrianLightEm.Instance.Subscribe(PedestrianLight.CreateInstance(new Vector2(600,600), 32, _peopleRed, _peopleGreen, _font));
            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 150), 24, _LightBikeRed, _LightBikeOrange,_LightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(150, 350), 23, _LightBikeRed, _LightBikeOrange,_LightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(375, 550), 22, _LightBikeRed, _LightBikeOrange,_LightBikeGreen, _font));            
            BicycleLightEm.Instance.Subscribe(BicycleLight.CreateInstance(new Vector2(550, 350), 21, _LightBikeRed, _LightBikeOrange,_LightBikeGreen, _font));            

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if(Keyboard.GetState().IsKeyDown(Keys.Y))
            {
                _server.Send();
                // Exit();
            }
            _camera.UpdateCamera(_graphics.GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Camera2D camera;
            // camera = new Camera2D(GraphicsDevice.Viewport);
            // Matrix viewMatrix = camera.GetViewMatrix();
            // _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: viewMatrix);
            // _spriteBatch.Begin(transformMatrix: _camera.Transform, blendState:BlendState.AlphaBlend, sortMode:SpriteSortMode.Deferred);
            _spriteBatch.Begin();
            
            EventManagerEm.Instance.Draw(_spriteBatch);
            
            // for (int i = 0; i < 30; ++i)
            // {
                // for (int j = 0; j < 30; ++j)
                // {
                    // _spriteBatch.Draw(_dirt, new Rectangle(i * 25, j * 25, 25, 25), Color.Aqua);
                // }
            // }
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
