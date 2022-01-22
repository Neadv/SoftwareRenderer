using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class Triangle
    {
        public int V0 { get; }
        public int V1 { get; }
        public int V2 { get; }
        public int N0 { get; set; }
        public int N1 { get; set; }
        public int N2 { get; set; }
        public Color Color { get; }

        public int UV0 { get; set; }
        public int UV1 { get; set; }
        public int UV2 { get; set; }

        public Triangle(int v0, int v1, int v2, int n0, int n1, int n2, Color color)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            N0 = n0;
            N1 = n1;
            N2 = n2;
            Color = color;

            UV0 = UV1 = UV2 = 0;
        }

        public Triangle(int v0, int v1, int v2, int n0, int n1, int n2, int uv0, int uv1, int uv2, Color color)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            N0 = n0;
            N1 = n1;
            N2 = n2;
            Color = color;

            UV0 = uv0;
            UV1 = uv1;
            UV2 = uv2;
        }
    }
}
