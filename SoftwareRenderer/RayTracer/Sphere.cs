using System;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.RayTracer
{
    public class Sphere
    {
        public Vector3f Position { get; set; }
        public float Radius { get; set; }
        public Color Color { get; set; }
        public float Specular { get; set; }
        public float Reflective { get; set; }

        public Sphere(Vector3f pos, float r, Color color, float specular, float reflective)
        {
            Position = pos;
            Radius = r;
            Color = color;
            Specular = specular;
            Reflective = Math.Clamp(reflective, 0, 1);;
        }
    }
}