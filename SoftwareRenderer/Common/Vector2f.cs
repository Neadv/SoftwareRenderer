using System;

namespace SoftwareRenderer.Common
{
    public struct Vector2f
    {
        public float X { get; set; }
        public float Y { get; set; }

        public Vector2f(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2f(float value)
        {
            X = Y = value;
        }

        public Vector2f Multiply(float value)
        {
            return new Vector2f(value * this.X, value * this.Y);
        }

        public Vector2f Divide(float value)
        {
            return new Vector2f(this.X / value, this.Y / value);
        }

        public Vector2f Add(Vector2f v)
        {
            return new Vector2f(this.X + v.X, this.Y + v.Y);
        }

        public Vector2f Substract(Vector2f v)
        {
            return new Vector2f(this.X - v.X, this.Y - v.Y);
        }

        public float Length()
        {
            return MathF.Sqrt(this.X * this.X + this.Y * this.Y);
        }

        public float LengthSquare()
        {
            return this.X * this.X + this.Y * this.Y;
        }

        public static Vector2f operator *(float value, Vector2f v) => v.Multiply(value);
        public static Vector2f operator *(Vector2f v, float value) => v.Multiply(value);
        public static Vector2f operator /(Vector2f v, float value) => v.Divide(value);
        public static Vector2f operator +(Vector2f v1, Vector2f v2) => v1.Add(v2);
        public static Vector2f operator -(Vector2f v1, Vector2f v2) => v1.Substract(v2);
        public static Vector2f operator -(Vector2f v) => v.Multiply(-1);
    }
}