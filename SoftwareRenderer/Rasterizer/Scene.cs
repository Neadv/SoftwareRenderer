using SoftwareRenderer.Common;
using SoftwareRenderer.Rasterizer.Models;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class Scene
    {
        public Camera Camera { get; set; }
        public Viewport Viewport { get; set; }
        public List<Model> Instances { get; set; } = new List<Model>();
        public List<Light> Lights { get; set; } = new List<Light>();

        public Scene()
        {

        }

        public Scene(Scene scene)
        {
            Instances = scene.Instances;
            Lights = scene.Lights;
        }
    }
}
