using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer.SceneBuilder
{
    public class LightOptionsBuilder
    {
        public Light Light => new Light(_position, _intensity, _lightType);

        private Vector3f _position;
        private float _intensity;
        private LightType _lightType;

        public LightOptionsBuilder SetPosition(Vector3f position)
        {
            _position = position;
            return this;
        }

        public LightOptionsBuilder SetIntensity(float intensity)
        {
            _intensity = intensity;
            return this;
        }

        public LightOptionsBuilder SetType(LightType lightType)
        {
            _lightType = lightType;
            return this;
        }
    }
}
