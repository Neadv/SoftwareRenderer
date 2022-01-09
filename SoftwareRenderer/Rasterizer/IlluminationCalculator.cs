using SoftwareRenderer.Common;
using System;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public static class IlluminationCalculator
    {
        private static float specular = 50;

        public static float ComputeIllumination(Vector3f vertex, Vector3f normal, Camera camera, IEnumerable<Light> lights)
        {
            float illumination = 0;
            foreach (var light in lights)
            {
                if (light.Type == LightType.Ambient)
                {
                    illumination += light.Intensity;
                    continue;
                }

                Vector3f vl = new Vector3f(0);
                if (light.Type == LightType.Directional)
                {
                    var cameraMatrix = camera.Orientation.Transpose();
                    vl = cameraMatrix * light.Position;
                }
                else if (light.Type == LightType.Point)
                {
                    var cameraMatrix = camera.Orientation.Transpose() * TransformHelper.MakeTranslationMatrix(-camera.Position);
                    var transformedLight = cameraMatrix * light.Position;
                    vl = transformedLight - vertex;
                }

                // Diffuse component
                var cosAlpha = vl.Dot(normal) / (vl.Length() * normal.Length());
                if (cosAlpha > 0)
                {
                    illumination += cosAlpha * light.Intensity;
                }

                // Specular component
                var reflected = vl.Reflect(normal);
                var view = camera.Position - vertex;

                var cosBeta = reflected.Dot(view) / (reflected.Length() * view.Length());
                if (cosBeta > 0)
                {
                    illumination += MathF.Pow(cosBeta, specular) * light.Intensity;
                }
            }


            return illumination;
        }
    }
}
