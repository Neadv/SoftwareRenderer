using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class Triangle
    {
        public int V1 { get; }
        public int V2 { get; }
        public int V3 { get; }
        public Color Color { get; }

        public Triangle(int v1, int v2, int v3, Color color)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            Color = color;
        }
    }
}
