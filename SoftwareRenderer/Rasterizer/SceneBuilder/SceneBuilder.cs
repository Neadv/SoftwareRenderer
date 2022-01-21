using SoftwareRenderer.Common;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareRenderer.Rasterizer.SceneBuilder
{
    public class SceneBuilder
    {
        private List<ModelOptionsBuilder> _modelOptionsBuilders = new List<ModelOptionsBuilder>();
        private List<LightOptionsBuilder> _lightOptionsBuilders = new List<LightOptionsBuilder>();
        private CameraOptionsBuilder _cameraOptionsBuilder = new CameraOptionsBuilder();
        private Viewport _viewport = new Viewport(1, 1, 1);

        public CameraOptionsBuilder AddCamera()
        {
            _cameraOptionsBuilder = new CameraOptionsBuilder();
            return _cameraOptionsBuilder;
        }

        public ModelOptionsBuilder AddModel()
        {
            var modelOptionsBuilder = new ModelOptionsBuilder();
            _modelOptionsBuilders.Add(modelOptionsBuilder);
            return modelOptionsBuilder;
        }

        public LightOptionsBuilder AddLight()
        {
            var lightOptionsBuilder = new LightOptionsBuilder();
            _lightOptionsBuilders.Add(lightOptionsBuilder);
            return lightOptionsBuilder;
        }

        public void SetupViewport(float width, float height, float distance)
        {
            _viewport = new Viewport(width, height, distance);
        }

        public Scene Build()
        {
            return new Scene
            {
                Viewport = _viewport,
                Camera = _cameraOptionsBuilder.Camera,
                Instances = _modelOptionsBuilders.Select(opts => opts.Instance).ToList(),
                Lights = _lightOptionsBuilders.Select(opts => opts.Light).ToList()
            };
        }
    }
}
