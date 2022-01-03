using SoftwareRenderer.Common;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class Mesh
    {
        public List<Common.Vector3f> Vertices { get; set; }
        public List<Triangle> Triangles { get; set; }

        public Mesh(List<Common.Vector3f> vertices, List<Triangle> triangles)
        {
            Vertices = vertices;
            Triangles = triangles;
        }

        public Mesh()
        {
            Vertices = new List<Common.Vector3f>();
            Triangles = new List<Triangle>();
        }
    }
}
