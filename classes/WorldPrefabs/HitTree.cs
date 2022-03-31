using System;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class HitTree
    {
        private Texture2D _textureHitTree;
        private int _landId;
        private int _currentFrame;

        public void Draw(SpriteBatch spriteBatch)
        {
            Console.WriteLine("hij komt hij komt die goeie sind");
        }

        public static HitTree CreateInstance(Vector2 pos)
        {
            return new HitTree();
        }
    }
}