using FactoryGame.Source.Systems.Abstracts;
using FactoryGame.Source;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using FactoryGame.Source.Systems;
using FactoryGame.Source.Models;

namespace FactoryGame.Source.Objects.World
{
    public class Chunk : InGameObject
    {
        public Location Id { get; }

        public Chunk(Location id)
        {
            Id = id;
        }

        protected override void Build(GameObject obj)
        {
            (Vector2[] vertices, int[] triangles, Vector2[] uv) = GetMeshData();

            var mesh = new Mesh();
            mesh.vertices = vertices.Expand();
            mesh.triangles = triangles;
            mesh.uv = uv;
            mesh.RecalculateNormals();

            if (obj == null || obj.gameObject == null || obj.activeSelf == false)
                return;

            obj.GetComponent<MeshFilter>().mesh = mesh;
            obj.GetComponent<Renderer>().material.SetTexture("_BaseMap", TextureHelper.Textures["World"]);
        }

        protected override (Vector3 position, Vector3 velocity) CalculateMovement() => (Position, Vector3.zero);

        protected override void OnStart()
        {

        }

        protected override void OnUpdate()
        {

        }

        //private Task<(Vector2[] vertices, int[] triangles, Vector2[] uv)> GetMeshData() => Task.Factory.StartNew(() =>
        private (Vector2[] vertices, int[] triangles, Vector2[] uv) GetMeshData()
        {
            var vertices = new List<Vector2>();
            var triangles = new List<int>();
            var uv = new List<Vector2>();

            for (int y = 0; y < 16; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    float s = 1f / 16f;
                    float xs = x * s;
                    float ys = y * s;

                    /*
                     * 
                     * 0 --- 2
                     * | # / |
                     * | /   |
                     * 1 --- x
                     * 
                     * x --- 4
                     * |   / |
                     * | / # |
                     * 5 --- 3
                     * 
                     * 
                     */

                    vertices.Add(new Vector2(xs, ys));
                    vertices.Add(new Vector2(xs, ys + s));
                    vertices.Add(new Vector2(xs + s, ys));

                    vertices.Add(new Vector2(xs + s, ys + s));
                    vertices.Add(new Vector2(xs + s, ys));
                    vertices.Add(new Vector2(xs, ys + s));

                    var tile = GameSystem.WorldData[((Id.X * 16) + x, (Id.Y * 16) + y)];
                    var uvLoc = TextureHelper.TileUV[tile].ToVector2() * s;
                    float xu = uvLoc.x;
                    float yu = uvLoc.y;

                    uv.Add(new Vector2(xu, yu));
                    uv.Add(new Vector2(xu, yu + s));
                    uv.Add(new Vector2(xu + s, yu));

                    uv.Add(new Vector2(xu + s, yu + s));
                    uv.Add(new Vector2(xu + s, yu));
                    uv.Add(new Vector2(xu, yu + s));

                    //uv.Add(new Vector2(0, 0));
                    //uv.Add(new Vector2(0, s));
                    //uv.Add(new Vector2(s, 0));

                    //uv.Add(new Vector2(s, s));
                    //uv.Add(new Vector2(s, 0));
                    //uv.Add(new Vector2(0, s));
                }
            }

            for (int t = 0; t < vertices.Count; t++)
                triangles.Add(t);

            return (vertices.ToArray(), triangles.ToArray(), uv.ToArray());
        }//);
    }
}