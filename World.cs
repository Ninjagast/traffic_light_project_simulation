using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using traffic_light_simulation.classes.Communication;
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

        public World()
        {
            EventManagerEm.Instance.Subscribe(TrafficLightEm.Instance);
            

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
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _dirt = Content.Load<Texture2D>("dirt");
            // TODO: use this.Content to load your game content here
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

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Camera2D camera;
            // camera = new Camera2D(GraphicsDevice.Viewport);
            // Matrix viewMatrix = camera.GetViewMatrix();
            // _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, transformMatrix: viewMatrix);
            _spriteBatch.Begin();
            
            
            _spriteBatch.Draw(_dirt, new Vector2(350,350), Color.White);
            
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }
    }
}
