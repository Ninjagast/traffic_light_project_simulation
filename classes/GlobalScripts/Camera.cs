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
        public int MovementSpeed { get; set; } = 20;
        public float Zoom { get; set; } = 0.6f;
        
        public Camera(Viewport viewport)
        {
            _bounds = viewport.Bounds;
            Pos = new Vector2(1000,1000);
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
                cameraMovement.Y = -MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                cameraMovement.Y = MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                cameraMovement.X = -MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                cameraMovement.X = MovementSpeed;
            }
            _moveCamera(cameraMovement);
        }

        public Vector2 GetPos()
        {
            Vector2 stabilizationVector = Vector2.Zero; 
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                stabilizationVector.Y = -MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                stabilizationVector.Y = MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                stabilizationVector.X = -MovementSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                stabilizationVector.X = MovementSpeed;
            }

            return Pos - stabilizationVector;
        }
    }
}