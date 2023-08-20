using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Raylib_cs;

namespace MonsterWorld
{
    class AssetManager
    {
        private static AssetManager _instance;

        private Dictionary<string, Texture2D> _textures = new();
        private Dictionary<string, Font> _fonts = new();
        private Dictionary<string, Wave> _waves = new();

        private AssetManager()
        {

        }

        public static AssetManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AssetManager();
                }

                return _instance;
            }
        }

        public Texture2D GetTexture(string name)
        {
            if (_textures.ContainsKey(name))
            {
                return _textures[name];
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"MonsterWorld.Assets.Textures.{name}"))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Could not find texture {name}");
                }

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    var image = Raylib.LoadImageFromMemory(".png", ms.ToArray());
                    var texture = Raylib.LoadTextureFromImage(image);
                    Raylib.UnloadImage(image);

                    _textures.Add(name, texture);

                    return texture;
                }
            }
        }

        public Font GetFont(string name, int size = 18)
        {
            if (_fonts.ContainsKey(name))
            {
                return _fonts[name];
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"MonsterWorld.Assets.Fonts.{name}"))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Could not find font {name}");
                }

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    var font = Raylib.LoadFontFromMemory(".ttf", ms.ToArray(), size, null, 0);

                    _fonts.Add(name, font);

                    return font;
                }
            }
        }

        public Wave GetWave(string name)
        {
            if (_waves.ContainsKey(name))
            {
                return _waves[name];
            }

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"MonsterWorld.Assets.Waves.{name}"))
            {
                if (stream == null)
                {
                    throw new FileNotFoundException($"Could not find wave {name}");
                }

                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    var wave = Raylib.LoadWaveFromMemory(".wav", ms.ToArray());

                    _waves.Add(name, wave);

                    return wave;
                }
            }
        }
    }
}
