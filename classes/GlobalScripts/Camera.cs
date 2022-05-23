using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace traffic_light_simulation.classes.GlobalScripts
{
    public class Camera
    {
        private Rectangle _bounds;
        private Vector2 _pos;
        public Matrix Transform;

        private int MovementSpeed { get; set; } = 20;
        private float Zoom { get; set; } = 0.6f;
        
        public Camera(Viewport viewport)
        {
            _bounds = viewport.Bounds;
            _pos = new Vector2(1000,700);
        }

        private void _updateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f, _bounds.Height * 0.5f, 0));
        }

        private void _moveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = _pos + movePosition;

            if (!(newPosition.X < 680 || newPosition.X > 2260))
            {
                _pos.X = newPosition.X;
            }

            if (!(newPosition.Y < 640 || newPosition.Y > 860))
            {
                _pos.Y = newPosition.Y;
            }
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
                if (_pos.Y != 640)
                {
                    stabilizationVector.Y = -MovementSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (_pos.Y != 860)
                {
                    stabilizationVector.Y = MovementSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                
                if (_pos.X != 680)
                {
                    stabilizationVector.X = -MovementSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (_pos.X != 2260)
                {
                    stabilizationVector.X = MovementSpeed;
                }
            }

            return _pos - stabilizationVector;
        }
    }
}