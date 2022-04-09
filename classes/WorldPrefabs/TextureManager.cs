using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace traffic_light_simulation.classes.WorldPrefabs
{
    public class TextureManager
    {
        private static TextureManager _instance;
        private static readonly object Padlock = new object();
        private TextureManager() {}
        public static TextureManager Instance
        {
            get
            {
                lock (Padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new TextureManager();
                    }
                    return _instance;
                }
            }
        }
        
        private Dictionary<string, Texture2D> _sedanTextures;
        private Dictionary<string, Texture2D> _trafficLightTextures;
        private Dictionary<string, Texture2D> _bicycleLightTextures;
        private Dictionary<string, Texture2D> _pedestrianLightTextures;

        public void SetTexture(Dictionary<string, Texture2D> textures, int id)
        {
            switch (id)
            {
                case 0:
                    _sedanTextures = textures;
                    break;

                case 1:
                    _trafficLightTextures = textures;
                    break;
                
                case 2:
                    _bicycleLightTextures = textures;
                    break;
                
                case 3:
                    _pedestrianLightTextures = textures;
                    break;
                
                default:
                    break;
            }
        }

        public Texture2D GetTexture(int id, string textureName)
        {
            switch (id)
            {
                case 0:
                    return _sedanTextures[textureName];

                case 1:
                    return _trafficLightTextures[textureName];
                
                case 2:
                    return _bicycleLightTextures[textureName];
                
                case 3:
                    return _pedestrianLightTextures[textureName];
                
                default:
                    break;
            }

            throw new ArgumentException($"There is not a texture with this id:{id}");
        }
    }
}