using FactoryGame.Source.Objects.World;
using FactoryGame.Source.Systems;
using FactoryGame.Source.Systems.Abstracts;
using UnityEngine;

namespace FactoryGame.Source
{
    public class GameSystem : MonoBehaviour
    {
        public static InGameObjectCollection LoadedObjects { get; } = new();
        public static bool Paused { get; set; } = false;

        public Texture WorldTexture { get; private set; }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            TextureHelper.Textures.Add("World", TextureHelper.LoadFromImage(@"Assets\Textures\world.png"));

            var chunk = new Chunk();
            chunk.Scale = new Vector2(16, 16);
            AddChild(chunk);
            LoadedObjects.Add(chunk);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddChild(InGameObject obj) =>
            obj.SetParent(transform);
    }
}

