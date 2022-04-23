using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace traffic_light_simulation.classes.GlobalScripts
{
    public class Camera
    {
        private Rectangle _bounds;
        public Vector2 Pos;
        public Matrix Transform;

//      todo these will be changeable using debug mode
        public int _movementSpeed { get; set; } = 10;
        public float Zoom { get; set; } = 0.8f;
        
        public Camera(Viewport viewport)
        {
            _bounds = viewport.Bounds;
            Pos = Vector2.Zero;
        }

        private void _updateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Pos.X, -Pos.Y, 0)) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f, _bounds.Height * 0.5f, 0));
        }

        private void _moveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = Pos + movePosition;
            Pos = newPosition;
        }

        public void UpdateCamera(Viewport bounds)
        {
            _bounds = bounds.Bounds;
            _updateMatrix();
            Vector2 cameraMovement = Vector2.Zero;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                cameraMovement.Y = -_movementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraMovement.Y = _movementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cameraMovement.X = -_movementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraMovement.X = _movementSpeed;
            }
            _moveCamera(cameraMovement);
        }
    }
}