using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace traffic_light_simulation.classes.GlobalScripts
{
    public class Camera
    {
        public float Zoom = 0.85f;
        public float Rotate;
        public Vector2 Position;
        public Rectangle Bounds;
        public Matrix Transform;

        private int _movementSpeed = 15;

        public Camera(Viewport viewport)
        {
            Bounds = viewport.Bounds;
            Position = Vector2.Zero;
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                        Matrix.CreateScale(Zoom) *
                        Matrix.CreateRotationZ(Rotate) *
                        Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
        }

        public void MoveCamera(Vector2 movePosition)
        {
            Vector2 newPosition = Position + movePosition;
            Position = newPosition;
        }

        public void UpdateCamera(Viewport bounds)
        {
            Bounds = bounds.Bounds;
            UpdateMatrix();
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
            MoveCamera(cameraMovement);
        }
    }
}