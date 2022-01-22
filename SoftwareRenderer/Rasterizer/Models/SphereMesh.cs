using SoftwareRenderer.Common;
using System;

namespace SoftwareRenderer.Rasterizer.Models
{
    public class SphereMesh : Mesh
    {
        public SphereMesh(float radius, Color color, int divs = 15)
        {
            float deltaAngle = 2 * MathF.PI * radius / divs;

            // Generate vertices and normals.
            for (int d = 0; d < divs + 1; d++)
            {
                float y = 2.0f / divs * (d - divs / 2.0f);
                float dRadius = MathF.Sqrt(radius - y * y);
                for (var i = 0; i < divs; i++)
                {
                    Vector3f vertex = new Vector3f(dRadius * MathF.Cos(i * deltaAngle), y, dRadius * MathF.Sin(i * deltaAngle));
                    Vertices.Add(vertex);
                }
            }

            // Generate triangles.
            for (int d = 0; d < divs; d++)
            {
                for (int i = 0; i < divs; i++)
                {
                    int i0 = d * divs + i;
                    int i1 = (d + 1) * divs + (i + 1) % divs;
                    int i2 = divs * d + (i + 1) % divs;

                    Normals.Add(Vertices[i0].Normalize());
                    Normals.Add(Vertices[i1].Normalize());
                    Normals.Add(Vertices[i2].Normalize());
                    Normals.Add(Vertices[i0 + divs].Normalize());

                    Triangles.Add(new Triangle(i0, i1, i2, Normals.Count - 4, Normals.Count - 3, Normals.Count - 2, color));
                    Triangles.Add(new Triangle(i0, i0 + divs, i1, Normals.Count - 4, Normals.Count - 1, Normals.Count - 3, color));
                }
            }

            CalculateBoundingSphere();
        }
    }
}
