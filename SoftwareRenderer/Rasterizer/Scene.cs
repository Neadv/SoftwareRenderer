using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class Scene
    {
        public List<Instance> Instances { get; set; } = new List<Instance>();

        public Scene()
        {

        }

        public Scene(Scene scene)
        {
            Instances = scene.Instances;
        }
    }
}
