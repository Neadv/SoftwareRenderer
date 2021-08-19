using System;

namespace SoftwareRenderer
{
    public struct Color
    {
        public static Color White => new Color(255, 255, 255);
        public static Color Black => new Color(0, 0, 0);
        public static Color Red => new Color(255, 0, 0);
        public static Color Green => new Color(0, 255, 0);
        public static Color Blue => new Color(0, 0, 255);

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public Color(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(float r, float g, float b, float a = 1)
        {
            r = Math.Clamp(r, 0, 1);
            g = Math.Clamp(g, 0, 1);
            b = Math.Clamp(b, 0, 1);
            a = Math.Clamp(a, 0, 1);

            R = (byte)(r * 255);
            G = (byte)(g * 255);
            B = (byte)(b * 255);
            A = (byte)(a * 255);
        }

        public Color(int color)
        {
            this.R = (byte)(color & 255);
            this.G = (byte)(color & (255 << 8));
            this.B = (byte)(color & (255 << 16));
            this.A = (byte)(color & (255 << 24));
        }

        public byte[] GetBytes()
        {
            return new byte[] { this.R, this.G, this.B, this.A }; // RGBA32
        }

        public Color Multiply(float value)
        {
            byte r = (byte)Math.Clamp(value * this.R, 0, 255);
            byte g = (byte)Math.Clamp(value * this.G, 0, 255);
            byte b = (byte)Math.Clamp(value * this.B, 0, 255);
            byte a = (byte)Math.Clamp(value * this.A, 0, 255);

            return new Color(r, g, b, a);
        }

        public Color Multiply(int value) => Multiply(value);

        public Color Add(Color color)
        {
            var r = (byte)Math.Clamp(this.R + color.R, 0, 255);
            var g = (byte)Math.Clamp(this.G + color.G, 0, 255);
            var b = (byte)Math.Clamp(this.B + color.B, 0, 255);
            var a = (byte)Math.Clamp(this.A + color.A, 0, 255);

            return new Color(r, g, b, a);
        }

        public Color Substract(Color color)
        {
            var r = (byte)Math.Clamp(this.R - color.R, 0, 255);
            var g = (byte)Math.Clamp(this.G - color.G, 0, 255);
            var b = (byte)Math.Clamp(this.B - color.B, 0, 255);
            var a = (byte)Math.Clamp(this.A - color.A, 0, 255);

            return new Color(r, g, b, a);
        }

        public static Color operator *(float value, Color color) => color.Multiply(value);
        public static Color operator *(int value, Color color) => color.Multiply(value);
        public static Color operator +(Color colorA, Color colorB) => colorA.Add(colorB);
        public static Color operator -(Color colorA, Color colorB) => colorA.Substract(colorB);
    }
}