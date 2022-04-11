using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class Camera
    {
        private float _zoom = 0.70f;
        private Vector2 _position;
        private Rectangle _bounds;
        public Matrix Transform;

        private int _movementSpeed = 10;

        public Camera(Viewport viewport)
        {
            _bounds = viewport.Bounds;
            _position = Vector2.Zero;
        }

        private void _updateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
                        Matrix.CreateScale(_zoom) *
                        Matrix.CreateTranslation(new Vector3(_bounds.Width * 0.5f, _bounds.Height * 0.5f, 0));
        }

        private void _moveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = _position + movePosition;
            _position = newPosition;
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