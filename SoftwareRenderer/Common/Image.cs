using System;

namespace SoftwareRenderer.Common
{
    public class Image
    {
        public byte[] Data { get; }

        public int Width { get; set; }
        public int Height { get; set; }

        public Image(byte[] data, int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException(nameof(width));

            if (height <= 0)
                throw new ArgumentException(nameof(width));

            if (height * width * 4 != data.Length)
                throw new ArgumentException(nameof(data));

            Data = data;
            Width = width;
            Height = height;
        }

        public Image(Color[] pixels, int width, int height)
        {
            if (width <= 0)
                throw new ArgumentException(nameof(width));

            if (height <= 0)
                throw new ArgumentException(nameof(width));

            if (height * width != pixels.Length)
                throw new ArgumentException(nameof(pixels));

            Data = new byte[width * height * 4];

            for (int i = 0; i < Data.Length; i++)
            {
                var pixel = pixels[i / 4];

                Data[i] = pixel.R;
                Data[++i] = pixel.G;
                Data[++i] = pixel.B;
                Data[++i] = pixel.A;
            }
        }

        public Color this[int x, int y]
        {
            get => Get(x, y);
        }

        public Color this[float x, float y]
        {
            get => Get(x, y);
        }

        public Color this[Vector2f uv]
        {
            get => Get(uv);
        }

        public Color Get(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return Color.Black;

            int index = 4 * (y * Width + x);

            return new Color(Data[index], Data[index + 1], Data[index + 2], Data[index + 3]);
        }

        public Color Get(float x, float y)
        {
            if (x < 0 || x > 1)
                throw new ArgumentOutOfRangeException(nameof(x));

            if (y < 0 || y > 1)
                throw new ArgumentOutOfRangeException(nameof(y));

            return Get((int)(x * Width), (int)(y * Height));
        }

        public Color Get(Vector2f uv)
        {
            return Get(uv.X, uv.Y);
        }
    }
}
