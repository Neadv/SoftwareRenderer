using System;
using System.Collections.Generic;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.RayTracer
{
    public class SoftwareRayTracer : IRenderer
    {
        private Vector3f _cameraPos;
        private Viewport _viewport;
        private Scene _scene;
        private ICanvas _canvas;

        public void Initialization(ICanvas canvas)
        {
            _canvas = canvas;
            _cameraPos = new Vector3f(0);
            _viewport = new Viewport(1, 1, 1);

            _scene = new Scene
            {
                Spheres = new List<Sphere>
                {
                    new Sphere(
                        pos: new Vector3f(0, -1, 3),
                        r: 1,
                        color: Color.Red,
                        specular: 500,
                        reflective: 0.2f
                    ),
                    new Sphere(
                        pos: new Vector3f(2, 0, 4),
                        r: 1,
                        color: Color.Blue,
                        specular: 500,
                        reflective: 0.3f
                    ),
                    new Sphere(
                        pos: new Vector3f(-2, 0, 4),
                        r: 1,
                        color: Color.Green,
                        specular: 10,
                        reflective: 0.4f
                    ),
                    new Sphere(
                        pos: new Vector3f(0, -5001, 0),
                        r: 5000,
                        color: new Color(255, 255, 0),
                        specular: 1000,
                        reflective: 0.5f
                    )
                },
                Lights = new List<Light>
                {
                    Light.CreateAmbient(0.2f),
                    Light.CreatePoint(0.6f, new Vector3f(2, 1, 0)),
                    Light.CreateDirectional(0.2f, new Vector3f(1, 4, 4))
                }
            };
        }

        public void Render()
        {
            for (int y = -_canvas.Height / 2; y <= _canvas.Height / 2; y++)
            {
                for (int x = -_canvas.Width / 2; x <= _canvas.Width / 2; x++)
                {
                    float posX = x * _viewport.Width / _canvas.Width;
                    float posY = y * _viewport.Height / _canvas.Height;
                    float posZ = _viewport.Distance;

                    var dir = (new Vector3f(posX, posY, posZ) - _cameraPos).Normalize();

                    var pixel = TraceRay(_cameraPos, dir, 3, 0.05f, float.MaxValue);
                    _canvas.Set(x, y, pixel);
                }
            }
        }

        private Color TraceRay(Vector3f center, Vector3f dir, int recursionDepth, float minDist = 0.01f, float maxDist = float.MaxValue)
        {
            var closestPoint = ClosestIntersection(center, dir, out var closestSphere, minDist, maxDist);
            if (closestSphere == null)
            {
                return Color.Black;
            }

            // local color
            var pos = center + closestPoint * dir;
            var normal = (pos - closestSphere.Position).Normalize();
            var intensity = ComputeLighting(pos, normal, -dir, closestSphere.Specular);
            var localColor = intensity * closestSphere.Color;

            if (recursionDepth <= 0 || closestSphere.Reflective <= 0)
            {
                return localColor;
            }

            var reflectedDir = -dir.Reflect(normal);
            var reflectedColor = TraceRay(pos, reflectedDir, recursionDepth - 1, minDist, maxDist);

            return localColor * (1 - closestSphere.Reflective) + reflectedColor * (closestSphere.Reflective); 
        }

        private float ClosestIntersection(Vector3f center, Vector3f dir, out Sphere closestSphere, float minDist = 0.01f, float maxDist = float.MaxValue)
        {
            float closestPoint = float.MaxValue;
            closestSphere = null;
            foreach (var sphere in _scene.Spheres)
            {
                (float t1, float t2) = IntersectRaySphere(center, dir, sphere);
                if (t1 > minDist && t1 < maxDist && t1 < closestPoint)
                {
                    closestPoint = t1;
                    closestSphere = sphere;
                }
                if (t2 > minDist && t2 < maxDist && t2 < closestPoint)
                {
                    closestPoint = t2;
                    closestSphere = sphere;
                }
            }
            return closestPoint;
        }

        private float ComputeLighting(Vector3f position, Vector3f normal, Vector3f view, float specular)
        {
            float i = 0;
            foreach (var light in _scene.Lights)
            {
                if (light.Type == LightType.Ambient)
                {
                    i += light.Intensity;
                }
                else
                {
                    Vector3f dir;
                    float maxDist;
                    if (light.Type == LightType.Directional)
                    {
                        dir = light.Position;
                        maxDist = float.MaxValue;
                    }
                    else
                    {
                        var v = light.Position - position;
                        dir = v.Normalize();
                        maxDist = v.Length();
                    }

                    // Shadow check
                    ClosestIntersection(position, dir, out var shadowSphere, 0.01f, maxDist);
                    if (shadowSphere != null)
                    {
                        continue;
                    }

                    // Diffuse
                    var dotDir = dir.Dot(normal);
                    if (dotDir > 0)
                    {
                        i += light.Intensity * dotDir;
                    }

                    // Specular
                    if (specular >= 0)
                    {
                        var reflect = dir.Reflect(normal);
                        var dotView = view.Dot(reflect);
                        if (dotView > 0)
                        {
                            i += light.Intensity * MathF.Pow(dotView, specular);
                        }
                    }
                }
            }

            return i;
        }

        private (float t1, float t2) IntersectRaySphere(Vector3f center, Vector3f dir, Sphere sphere)
        {
            var r = sphere.Radius;
            var co = center - sphere.Position;

            var a = dir.Dot(dir);
            var b = 2 * co.Dot(dir);
            var c = co.Dot(co) - r * r;

            var d = b * b - 4 * a * c;
            if (d < 0)
            {
                return (float.MaxValue, float.MaxValue);
            }

            var t1 = (-b + MathF.Sqrt(d)) / (2 * a);
            var t2 = (-b - MathF.Sqrt(d)) / (2 * a);

            return (t1, t2);
        }
    }
}