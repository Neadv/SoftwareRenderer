using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class Triangle
    {
        public int V0 { get; }
        public int V1 { get; }
        public int V2 { get; }
        public Vector3f N0 { get; set; }
        public Vector3f N1 { get; set; }
        public Vector3f N2 { get; set; }
        public Color Color { get; }

        public Vector2f UV0 { get; set; }
        public Vector2f UV1 { get; set; }
        public Vector2f UV2 { get; set; }

        public Triangle(int v0, int v1, int v2, Vector3f n0, Vector3f n1, Vector3f n2, Color color)
        {
            V0 = v0;
            V1 = v1;
            V2 = v2;
            N0 = n0;
            N1 = n1;
            N2 = n2;
            Color = color;

            UV0 = UV1 = UV2 = new Vector2f(0);
        }

        public Triangle(int v0, int v1, int v2, Vector3f n0, Vector3f n1, Vector3f n2, Vector2f uv0, Vector2f uv1, Vector2f uv2, Color color)
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
