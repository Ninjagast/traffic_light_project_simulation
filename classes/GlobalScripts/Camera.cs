using System;
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
            Pos = new Vector2(1000,700);
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

            if (!(newPosition.X < 680 || newPosition.X > 2260))
            {
                Pos.X = newPosition.X;
            }

            if (!(newPosition.Y < 640 || newPosition.Y > 860))
            {
                Pos.Y = newPosition.Y;
            }
            
            Console.WriteLine(Pos);
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
                if (Pos.Y != 640)
                {
                    stabilizationVector.Y = -MovementSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (Pos.Y != 860)
                {
                    stabilizationVector.Y = MovementSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                
                if (Pos.X != 680)
                {
                    stabilizationVector.X = -MovementSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (Pos.X != 2260)
                {
                    stabilizationVector.X = MovementSpeed;
                }
            }

            return Pos - stabilizationVector;
        }
    }
}