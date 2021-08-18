namespace SoftwareRenderer
{
    public class Canvas
    {
        public int Width { get; }
        public int Height { get; }

        public byte[] Bytes { get; private set; }

        public Canvas(int width, int heihgt)
        {
            Width = width;
            Height = heihgt;

            Bytes = new byte[width * heihgt * 4];
        }

        public Color this[int x, int y]
        {
            get => GetColor(x, y);
            set => SetColor(x, y, value);
        }

        public void SetColor(int x, int y, Color color)
        {
            x = Width / 2 + x;
            y = Height / 2 - y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return;
            
            int index = y * Width + x;

            // BGRA32
            Bytes[index] = color.B;
            Bytes[index + 1] = color.G;
            Bytes[index + 2] = color.R;
            Bytes[index + 3] = color.A;
        }

        public Color GetColor(int x, int y)
        {
            x = Width / 2 + x;
            y = Height / 2 - y;

            if (x < 0 || x >= Width || y < 0 || y >= Height)
                return Color.Black;
            
            int index = y * Width + x;
            
            return new Color(Bytes[index + 2], Bytes[index + 1], Bytes[index], Bytes[index + 3]);
        }
    }
}