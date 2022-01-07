using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public struct Triangle
    {
        public int V0 { get; }
        public int V1 { get; }
        public int V2 { get; }
        public Color Color { get; }

        public Triangle(int v0, int v1, int v2, Color color)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            Color = color;
        }
    }
}
