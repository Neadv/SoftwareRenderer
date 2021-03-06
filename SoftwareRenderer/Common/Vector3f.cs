using System;

namespace SoftwareRenderer.Common
{
    public struct Vector3f
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3f(float value)
        {
            X = Y = Z = value;
        }

        public override string ToString()
        {
            return $"X: {X}; Y: {Y}; Z: {Z}; |V|={Length()}";
        }

        public Vector3f Multiply(float value)
        {
            return new Vector3f(value * this.X, value * this.Y, value * this.Z);
        }

        public Vector3f Divide(float value)
        {
            return new Vector3f(this.X / value, this.Y / value, this.Z / value);
        }

        public Vector3f Add(Vector3f v)
        {
            return new Vector3f(this.X + v.X, this.Y + v.Y, this.Z + v.Z);
        }

        public Vector3f Substract(Vector3f v)
        {
            return new Vector3f(this.X - v.X, this.Y - v.Y, this.Z - v.Z);
        }

        public float Dot(Vector3f v)
        {
            return this.X * v.X + this.Y * v.Y + this.Z * v.Z;
        }

        public Vector3f Cross(Vector3f v)
        {
            return new Vector3f
            (
                x: this.Y * v.Z - this.Z * v.Y,
                y: this.Z * v.X - this.X * v.Z,
                z: this.X * v.Y - this.Y * v.X
            );
        }

        public float Length()
        {
            return MathF.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);
        }

        public Vector3f Normalize()
        {
            return this / this.Length();
        }

        public Vector3f Reflect(Vector3f normal)
        {
            return 2 * normal * normal.Dot(this) - this;
        }

        public float LengthSquare()
        {
            return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
        }

        public static Vector3f operator *(float value, Vector3f v) => v.Multiply(value);
        public static Vector3f operator *(Vector3f v, float value) => v.Multiply(value);
        public static Vector3f operator /(Vector3f v, float value) => v.Divide(value);
        public static Vector3f operator +(Vector3f v1, Vector3f v2) => v1.Add(v2);
        public static Vector3f operator -(Vector3f v1, Vector3f v2) => v1.Substract(v2);
        public static Vector3f operator -(Vector3f v) => v.Multiply(-1);
        public static float operator *(Vector3f v1, Vector3f v2) => v1.Dot(v2);
    }
}