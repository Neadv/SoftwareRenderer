using System.Collections.Generic;

namespace SoftwareRenderer.RayTracer
{
    public class Scene
    {
        public List<Sphere> Spheres { get; set; }
        public List<Light> Lights { get; set; }

        public Scene()
        {
            Spheres = new List<Sphere>();
            Lights = new List<Light>();
        }
    }
}