using SoftwareRenderer.Common;

namespace SoftwareRenderer.RayTracer
{
    public class Sphere
    {
        public Vector3f Position { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }

        public Sphere(Vector3f pos, float r, Color color)
        {
            Position = pos;
            Radius = r;
            Color = color;
        }
    }
}