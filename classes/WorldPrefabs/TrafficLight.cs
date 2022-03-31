using System;
using System.Net.Mime;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class TrafficLight: IDrawAble
    {
        private Texture2D _textureRed;
        private Texture2D _textureOrange;
        private Texture2D _textureGreen;
        private int _laneId;
        private Vector2 _pos;
        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void StateChange(int id, Enum state)
        {
            throw new NotImplementedException();
        }

        public static TrafficLight CreateInstance(Vector2 pos, int routeId, Texture2D textureRed, Texture2D textureOrange, Texture2D textureGreen)
        {
            TrafficLight returnInstance = new TrafficLight();
            returnInstance._laneId = routeId;
            returnInstance._pos = pos;
            returnInstance._textureGreen = textureGreen;
            returnInstance._textureOrange = textureOrange;
            returnInstance._textureRed = textureRed;
            return returnInstance;
        }
    }
}