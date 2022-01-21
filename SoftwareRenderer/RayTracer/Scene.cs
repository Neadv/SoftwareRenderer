using SoftwareRenderer.Common;
using System.Collections.Generic;

namespace SoftwareRenderer.RayTracer
{
    public class Scene
    {
        public List<Sphere> Spheres { get; set; }
        public List<Light> Lights { get; set; }
        public Vector3f CameraPosition { get; set; }
        public Viewport Viewport { get; set; }

        public Scene()
        {
            Spheres = new List<Sphere>();
            Lights = new List<Light>();
            CameraPosition = new Vector3f();
            Viewport = new Viewport(1, 1, 1);
        }
    }
}