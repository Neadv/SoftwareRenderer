using SoftwareRenderer.Common;
using System;

namespace SoftwareRenderer.Rasterizer
{
    public class ZBuffer : IBuffer<float>
    {
        private float[] _values;

        public ZBuffer(int width, int height)
        {
            _values = new float[width * height];
            Array.Fill(_values, 0);

            Width = width;
            Height = height;
        }

        public float this[int x, int y] 
        { 
            get => Get(x, y); 
            set => Set(x, y, value); 
        }

        public int Width { get; }

        public int Height { get; }

        public float Get(int x, int y)
        {
            x = Width / 2 + x;
            y = Height / 2 - y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return 0;

            return _values[y * Height + x];
        }

        public float Get(Vector2i point)
        {
            return Get(point.X, point.Y);
        }

        public void Set(int x, int y, float value)
        {
            x = Width / 2 + x;
            y = Height / 2 - y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            _values[y * Height + x] = value;
        }

        public void Set(Vector2i point, float value)
        {
            Set(point.X, point.Y, value);
        }
    }
}
