using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using traffic_light_simulation.classes.enums;
using traffic_light_simulation.classes.GlobalScripts;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class Car: IDrawAble
    {
        private Texture2D _texture;
        private Vector2 _pos;
        private float _direction;
        private States _state;
        private List<int> _directionMap;
        private int _currentFrame;
        private int id = -1;
        
        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
//          todo need to change the size based on texture also add rotation to the draw parameter based on direction
            spriteBatch.Draw(_texture, new Rectangle((int)_pos.X, (int)_pos.Y, 20, 50), Color.White);
        }

        public void StateChange(int id, States state)
        {
            _state = state;
        }

        public static Car CreateInstance(Texture2D texture2D)
        {
            Car returnObject = new Car();
            returnObject._pos = SpawnPoints.Instance.GetRandomLandSpawnPoint();
            returnObject._texture = texture2D;
            returnObject._state = States.IDLE;
            returnObject._currentFrame = 0;
//          todo generate this list based on waypoint system
            returnObject._directionMap = new List<int>();
            return returnObject;
        }
    }
}