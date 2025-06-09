using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FactoryGame.Source
{
    public static class Extensions
    {
        public static Vector3[] Expand(this Vector2[] arr, float dim = 0) =>
            arr.Select(x => new Vector3(x.x, x.y, dim)).ToArray();
    }
}
