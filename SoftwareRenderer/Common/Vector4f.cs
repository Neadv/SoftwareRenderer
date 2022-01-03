using System;

namespace SoftwareRenderer.Common
{
    public struct Vector4f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float W { get; set; }

        public Vector4f(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4f(float value)
        {
            X = Y = Z = W = value;
        }

        public Vector4f Multiply(float value)
        {
            return new Vector4f(value * this.X, value * this.Y, value * this.Z, value * this.W);
        }

        public Vector4f Divide(float value)
        {
            return new Vector4f(this.X / value, this.Y / value, this.Z / value, this.W / value);
        }

        public Vector4f Add(Vector4f v)
        {
            return new Vector4f(this.X + v.X, this.Y + v.Y, this.Z + v.Z, this.W + v.W);
        }

        public Vector4f Substract(Vector4f v)
        {
            return new Vector4f(this.X - v.X, this.Y - v.Y, this.Z - v.Z, this.W - v.W);
        }

        public static Vector4f operator *(float value, Vector4f v) => v.Multiply(value);
        public static Vector4f operator *(Vector4f v, float value) => v.Multiply(value);
        public static Vector4f operator /(Vector4f v, float value) => v.Divide(value);
        public static Vector4f operator +(Vector4f v1, Vector4f v2) => v1.Add(v2);
        public static Vector4f operator -(Vector4f v1, Vector4f v2) => v1.Substract(v2);
        public static Vector4f operator -(Vector4f v) => v.Multiply(-1);
    }
}