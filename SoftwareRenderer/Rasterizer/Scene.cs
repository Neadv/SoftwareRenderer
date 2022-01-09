using SoftwareRenderer.Common;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class Scene
    {
        public List<Instance> Instances { get; set; }
        public List<Light> Lights { get; set; }

        public Scene()
        {
            Instances = new List<Instance>();
            Lights = new List<Light>();
        }

        public Scene(Scene scene)
        {
            Instances = scene.Instances;
            Lights = scene.Lights;
        }
    }
}
