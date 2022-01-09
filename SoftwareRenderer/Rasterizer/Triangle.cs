using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public struct Triangle
    {
        public int V0 { get; }
        public int V1 { get; }
        public int V2 { get; }
        public Vector3f N0 { get; set; }
        public Vector3f N1 { get; set; }
        public Vector3f N2 { get; set; }
        public Color Color { get; }

        public Triangle(int v0, int v1, int v2, Vector3f n0, Vector3f n1, Vector3f n2, Color color)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            N0 = n0;
            N1 = n1;
            N2 = n2;
            Color = color;
        }
    }
}
