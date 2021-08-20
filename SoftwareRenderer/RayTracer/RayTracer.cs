using System;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.RayTracer
{
    public class SoftwareRayTracer : IRenderer
    {
        private Vector3f _cameraPos;
        private Viewport _viewport;
        private Sphere[] _spheres;

        public void Initialization()
        {
            _cameraPos = new Vector3f(0);
            _viewport = new Viewport(1, 1, 1);

            _spheres = new Sphere[] {
                new Sphere(
                    new Vector3f(0, -1, 3),
                    1,
                    Color.Red
                ),
                new Sphere(
                    new Vector3f(2, 0, 4),
                    1,
                    Color.Blue
                ),
                new Sphere(
                    new Vector3f(-2, 0, 4),
                    1,
                    Color.Green
                ),
            };
        }

        public void Render(ICanvas canvas)
        {
            for (int y = -canvas.Height / 2; y < canvas.Height / 2; y++)
            {
                for (int x = -canvas.Width / 2; x < canvas.Width / 2; x++)
                {
                    float posX = x * _viewport.Width / canvas.Width;
                    float posY = y * _viewport.Height / canvas.Height;
                    float posZ = _viewport.Distance;

                    var dir = (new Vector3f(posX, posY, posZ) - _cameraPos).Normalize();

                    var pixel = TraceRay(_cameraPos, dir);
                    canvas.SetColor(x, y, pixel);
                }
            }
        }

        private Color TraceRay(Vector3f center, Vector3f dir, float minDist = 1, float maxDist = float.MaxValue)
        {
            float closestPoint = float.MaxValue;
            Sphere closestSphere = null;
            foreach (var sphere in _spheres)
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
            if (closestSphere == null)
            {
                return Color.White;
            }
            return closestSphere.Color;
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