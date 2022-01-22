using SoftwareRenderer.Common;
using SoftwareRenderer.Utils;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer.Models
{
    public class Mesh
    {
        public List<Vector3f> Vertices { get; set; }
        public List<Vector3f> Normals { get; set; }
        public List<Vector2f> UVs { get; set; }
        public List<Triangle> Triangles { get; set; }

        public Sphere BoundingSphere { get; private set; }
        
        public Mesh()
            : this(new List<Vector3f>(), new List<Triangle>(), new List<Vector3f>(), new List<Vector2f>())
        { }

        public Mesh(List<Vector3f> vertices, List<Triangle> triangles, List<Vector3f> normals, List<Vector2f> uvs)
        {
            Vertices = vertices;
            Triangles = triangles;
            UVs = uvs;
            Normals = normals;

            CalculateBoundingSphere();
        }

        public void Transform(Matrix4x4 transform)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                Vertices[i] = transform * Vertices[i];
            }
            BoundingSphere = new Sphere(transform * BoundingSphere.Center, BoundingSphere.R);
        }

        protected void CalculateBoundingSphere()
        {
            BoundingSphere = MathHelper.CalculateBoundingSphere(Vertices);
        }


        public static Mesh Copy(Mesh mesh)
        {
            return new Mesh
            {
                Vertices = new List<Vector3f>(mesh.Vertices),
                Triangles = new List<Triangle>(mesh.Triangles),
                Normals = new List<Vector3f>(mesh.Normals),
                UVs = new List<Vector2f>(mesh.UVs),
                BoundingSphere = mesh.BoundingSphere
            };
        }
    }
}
