namespace SoftwareRenderer.Common
{
    public class Canvas : ICanvas
    {
        public int Width { get; }
        public int Height { get; }

        public byte[] Bytes { get; private set; }

        public Canvas(int width, int height)
        {
            Width = width;
            Height = height;

            Bytes = new byte[width * height * 4];
        }

        public Color this[int x, int y]
        {
            get => Get(x, y);
            set => Set(x, y, value);
        }

        public void Set(int x, int y, Color color)
        {
            x = Width / 2 + x;
            y = Height / 2 - y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;

            int index = 4 * (y * Width + x);

            // RGBA32
            Bytes[index] = color.R;
            Bytes[index + 1] = color.G;
            Bytes[index + 2] = color.B;
            Bytes[index + 3] = color.A;
        }

        public Color Get(int x, int y)
        {
            x = Width / 2 + x;
            y = Height / 2 - y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return Color.Black;

            int index = 4 * (y * Width + x);

            return new Color(Bytes[index], Bytes[index + 1], Bytes[index + 2], Bytes[index + 3]);
        }

        public void Clear(Color color)
        {
            for (int y = -Height / 2; y <= Height / 2; y++)
            {
                for (int x = -Width / 2; x <= Width / 2; x++)
                {
                    Set(x, y, color);
                }
            }
        }

        public void Set(Vector2i point, Color color) => Set(point.X, point.Y, color);

        public Color Get(Vector2i point) => Get(point.X, point.Y);
    }
}