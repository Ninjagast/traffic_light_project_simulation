using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;


namespace traffic_light_simulation.classes.GlobalScripts
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
        
        private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
        
        private Dictionary<string, Texture2D> _buttons = new Dictionary<string, Texture2D>();
        private Dictionary<string, Texture2D> _debugTextures = new Dictionary<string, Texture2D>();
        private Dictionary<string, SpriteFont> _fonts = new Dictionary<string, SpriteFont>();

        public void AddButtonTexture(Texture2D texture2D, string buttonName)
        {
            _buttons.Add(buttonName, texture2D);
        }

        public void AddDebugTexture(Texture2D texture2D, string name)
        {
            _debugTextures.Add(name, texture2D);
        }
        
        public void AddFont(SpriteFont font, string name)
        {
            _fonts.Add(name, font);
        }
        
        public void SetTexture(Dictionary<string, Texture2D> textures)
        {
            foreach (var texture in textures)
            {
                _textures.Add(texture.Key, texture.Value);
            }
        }

        public void SetTexture(Texture2D texture, string name)
        {
            _textures.Add(name, texture);
        }

        public Texture2D GetTexture(string textureName)
        {

            if (_textures.ContainsKey(textureName))
            {
                return _textures[textureName];
            }
            throw new ArgumentException($"There is not a texture with this name: {textureName}");
        }

        public SpriteFont GetFont(string name = "SmallFont")
        {
            return _fonts[name];
        }

        public Texture2D GetButtonTexture(string name)
        {
            if (_buttons.ContainsKey(name))
            {
                return _buttons[name];
            }

            throw new Exception($"This button name: {name} does not exist");
        }

        public Texture2D GetDebugTexture(string name)
        {
            if (_debugTextures.ContainsKey(name))
            {
                return _debugTextures[name];
            }
            throw new Exception($"Debug texture with button name: {name} does not exist");
        }
    }
}