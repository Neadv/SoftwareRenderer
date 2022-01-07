using SoftwareRenderer.Common;
using SoftwareRenderer.Utils;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class Mesh
    {
        public List<Vector3f> Vertices { get; set; }
        public List<Triangle> Triangles { get; set; }

        public Sphere BoundingSphere { get; private set;  }

        public Mesh(List<Vector3f> vertices, List<Triangle> triangles)
        {
            Vertices = vertices;
            Triangles = triangles;

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
            BoundingSphere = MathHelper.CalculateBoundingSphere(this.Vertices);
        }

        public Mesh()
            : this(new List<Vector3f>(), new List<Triangle>())
        { }

        public static Mesh Copy(Mesh mesh)
        {
            return new Mesh
            {
                Vertices = new List<Vector3f>(mesh.Vertices),
                Triangles = new List<Triangle>(mesh.Triangles),
                BoundingSphere = mesh.BoundingSphere
            };
        }
    }
}
