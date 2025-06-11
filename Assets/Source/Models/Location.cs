using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FactoryGame.Source.Models
{
    public struct Location : IEquatable<Location>, IEquatable<(int, int)>, IFormattable
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }

        public Vector2 ToVector2() =>
            new Vector2(X, Y);

        public static Location ClampVector(Vector3 vector) =>
            new Location((int)Math.Ceiling(vector.x), (int)Math.Ceiling(vector.y));

        public Location AddX(int a) => new Location(this.X + a, this.Y);
        public Location AddY(int a) => new Location(this.X, this.Y + a);

        public bool Equals(Location other) => this.X == other.X && this.Y == other.Y;
        public bool Equals((int, int) other) => this.X == other.Item1 && this.Y == other.Item2;
        public override bool Equals(object obj) =>
            obj is Location && Equals((Location)obj);

        public override int GetHashCode() =>
            HashCode.Combine(X, Y);

        public static Location operator +(Location a) => a;
        public static Location operator -(Location a) => new Location(-a.X, -a.Y);
        public static Location operator +(Location a, Location b) => new Location(a.X + b.X, a.Y + b.Y);
        public static Location operator +(Location a, (int, int) b) => new Location(a.X + b.Item1, a.Y + b.Item2);
        public static Location operator -(Location a, Location b) => new Location(a.X - b.X, a.Y - b.Y);
        public static Location operator -(Location a, (int, int) b) => new Location(a.X - b.Item1, a.Y - b.Item2);
        public static Location operator *(Location a, Location b) => new Location(a.X * b.X, a.Y * b.Y);
        public static Location operator *(Location a, (int, int) b) => new Location(a.X * b.Item1, a.Y * b.Item2);
        public static Location operator /(Location a, Location b) => new Location(a.X / b.X, a.Y / b.Y);
        public static Location operator /(Location a, (int, int) b) => new Location(a.X / b.Item1, a.Y / b.Item2);
        public static Location operator +(Location a, int b) => new Location(a.X + b, a.Y + b);
        public static Location operator -(Location a, int b) => new Location(a.X - b, a.Y - b);
        public static Location operator *(Location a, int b) => new Location(a.X * b, a.Y * b);
        public static Location operator /(Location a, int b) => new Location(a.X / b, a.Y / b);
        public static bool operator ==(Location a, Location b) => a.Equals(b);
        public static bool operator ==(Location a, (int, int) b) => a.Equals(b);
        public static bool operator !=(Location a, Location b) => !a.Equals(b);
        public static bool operator !=(Location a, (int, int) b) => !a.Equals(b);

        public static implicit operator (int, int)(Location a) => (a.X, a.Y);
        public static implicit operator Location((int, int) a) => new Location(a.Item1, a.Item2);
        public override string ToString() => $"{X},{Y}";

        public string ToString(string format)
        {
            if (string.IsNullOrEmpty(format))
                format = "X,Y";

            return format.Replace("X", X.ToString()).Replace("Y", Y.ToString());
        }

        public string ToString(string format, IFormatProvider formatProvider) => ToString(format);

        public Location[] RangeCombineLocation(Location[] locations) =>
            RangeCombineVector(locations.Select(x => x.ToVector2()).ToArray()).Select(x => ClampVector(x)).ToArray();

        public Vector2[] RangeCombineVector(Vector2[] vectors)
        {
            var list = new List<Vector2>();
            foreach (var vector in vectors)
                list.Add(vector + ToVector2());
            return list.ToArray();
        }
    }
}
