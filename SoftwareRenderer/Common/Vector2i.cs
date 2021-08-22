using System;

namespace SoftwareRenderer.Common
{
    public struct Vector2i
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector2i(int value)
        {
            X = Y = value;
        }

        public Vector2i Multiply(int value)
        {
            return new Vector2i(value * this.X, value * this.Y);
        }

        public Vector2i Divide(int value)
        {
            return new Vector2i(this.X / value, this.Y / value);
        }

        public Vector2i Add(Vector2i v)
        {
            return new Vector2i(this.X + v.X, this.Y + v.Y);
        }

        public Vector2i Substract(Vector2i v)
        {
            return new Vector2i(this.X - v.X, this.Y - v.Y);
        }

        public float Length()
        {
            return MathF.Sqrt(this.X * this.X + this.Y * this.Y);
        }

        public float LengthSquare()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public static Vector2i operator *(int value, Vector2i v) => v.Multiply(value);
        public static Vector2i operator *(Vector2i v, int value) => v.Multiply(value);
        public static Vector2i operator /(Vector2i v, int value) => v.Divide(value);
        public static Vector2i operator +(Vector2i v1, Vector2i v2) => v1.Add(v2);
        public static Vector2i operator -(Vector2i v1, Vector2i v2) => v1.Substract(v2);
        public static Vector2i operator -(Vector2i v) => v.Multiply(-1);
    }
}