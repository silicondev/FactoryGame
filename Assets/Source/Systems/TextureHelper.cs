using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FactoryGame.Source.Systems
{
    public static class TextureHelper
    {
        public static Dictionary<string, Texture2D> Textures { get; } = new();

        public static Texture2D LoadFromImage(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("File does not exist.");

            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.alphaIsTransparency = true;
            texture.filterMode = FilterMode.Point;
            texture.LoadImage(bytes);
            return texture;
        }
    }
}
