using FactoryGame.Source.Objects.World;
using FactoryGame.Source.Systems;
using FactoryGame.Source.Systems.Abstracts;
using FactoryGame.Source.Models;
using UnityEngine;
using System.Collections.Generic;

namespace FactoryGame.Source
{
    public class GameSystem : MonoBehaviour
    {
        public static InGameObjectCollection LoadedObjects { get; } = new();
        public static bool Paused { get; set; } = false;

        public static Dictionary<Location, TileType> WorldData = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            TextureHelper.Textures.Add("World", TextureHelper.LoadFromImage(@"Assets\Textures\world.png"));

            for (int cy = 0; cy < 8; cy++)
            {
                for (int cx = 0; cx < 8; cx++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        for (int x = 0; x < 16; x++)
                        {
                            var tileType = (TileType)Random.Range(1, 4);
                            WorldData.Add(((cx * 16) + x, (cy * 16) + y), tileType);
                        }
                    }
                    var c = new Chunk((cx, cy));
                    c.Position = new Vector3(cx * 16, cy * 16, 0);
                    c.Scale = new Vector2(16, 16);
                    AddChild(c);
                    LoadedObjects.Add(c);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddChild(InGameObject obj) =>
            obj.SetParent(transform);
    }
}

