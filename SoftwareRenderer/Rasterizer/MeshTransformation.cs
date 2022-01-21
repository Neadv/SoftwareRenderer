using SoftwareRenderer.Common;
using SoftwareRenderer.Rasterizer.Models;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public static class MeshTransformation
    {
        public static Mesh TransformAndClip(Mesh mesh, Matrix4x4 transform, IEnumerable<Plane> clippingPlanes)
        {
            Vector3f center = transform * mesh.BoundingSphere.Center;
            float radius2 = mesh.BoundingSphere.R * mesh.BoundingSphere.R;
            foreach (var clippedPlane in clippingPlanes)
            {
                float distance2 = clippedPlane.Normal.Dot(center) + clippedPlane.Distance;
                if (distance2 < -radius2)
                {
                    return null;
                }
            }

            var transformedMesh = Mesh.Copy(mesh);
            transformedMesh.Transform(transform);
            foreach (var plane in clippingPlanes)
            {
                var newTriangles = new List<Triangle>(mesh.Triangles.Count);
                foreach (var triangle in transformedMesh.Triangles)
                {
                    ClipTriangle(triangle, plane, transformedMesh.Vertices, newTriangles);
                }
                transformedMesh.Triangles = newTriangles;
            }

            return transformedMesh;
        }

        private static void ClipTriangle(Triangle triangle, Plane plane, List<Vector3f> vertices, IList<Triangle> triangles)
        {
            Vector3f v0 = vertices[triangle.V0];
            Vector3f v1 = vertices[triangle.V1];
            Vector3f v2 = vertices[triangle.V2];

            int inCount = 0;
            inCount += plane.Normal * v0 + plane.Distance > 0 ? 1 : 0;
            inCount += plane.Normal * v1 + plane.Distance > 0 ? 1 : 0;
            inCount += plane.Normal * v2 + plane.Distance > 0 ? 1 : 0;

            if (inCount == 3)
            {
                triangles.Add(triangle);
            }
            else if (inCount == 1)
            {
                // The triangle has one vertex in. Output is one clipped triangle.
            }
            else if (inCount == 2)
            {
                // The triangle has two vertices in. Output is two clipped triangles.
            }
        }
    }
}
