using System.Collections.Generic;

namespace SoftwareRenderer.RayTracer
{
    public class Scene
    {
        public List<Sphere> Spheres { get; set; }

        public Scene()
        {
            Spheres = new List<Sphere>();
        }

        public Scene(IEnumerable<Sphere> spheres)
        {
            Spheres = new List<Sphere>(spheres);
        }
    }
}