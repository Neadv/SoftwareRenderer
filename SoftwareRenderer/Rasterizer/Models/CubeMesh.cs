using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer.Models
{
    public class CubeMesh : Mesh
    {
        public CubeMesh(float size, Color color)
            : this(size, color, color, color, color, color, color)
        { }

        public CubeMesh(float size, Color color1, Color color2, Color color3, Color color4, Color color5, Color color6)
        {
            float halfSize = size / 2;
            Vertices.Add(new Vector3f(halfSize, halfSize, halfSize));
            Vertices.Add(new Vector3f(-halfSize, halfSize, halfSize));
            Vertices.Add(new Vector3f(-halfSize, -halfSize, halfSize));
            Vertices.Add(new Vector3f(halfSize, -halfSize, halfSize));
            Vertices.Add(new Vector3f(halfSize, halfSize, -halfSize));
            Vertices.Add(new Vector3f(-halfSize, halfSize, -halfSize));
            Vertices.Add(new Vector3f(-halfSize, -halfSize, -halfSize));
            Vertices.Add(new Vector3f(halfSize, -halfSize, -halfSize));

            Normals.Add(new Vector3f(0, 0, 1));
            Normals.Add(new Vector3f(1, 0, 0));
            Normals.Add(new Vector3f(0, 0, -1));
            Normals.Add(new Vector3f(-1, 0, 0));
            Normals.Add(new Vector3f(0, 1, 0));
            Normals.Add(new Vector3f(0, -1, 0));

            UVs.Add(new Vector2f(0, 0));
            UVs.Add(new Vector2f(1, 0));
            UVs.Add(new Vector2f(1, 1));
            UVs.Add(new Vector2f(0, 1));

            Triangles.Add(new Triangle(0, 1, 2, 0, 0, 0, 0, 1, 2, color1));
            Triangles.Add(new Triangle(0, 2, 3, 0, 0, 0, 0, 2, 3, color1));
            Triangles.Add(new Triangle(4, 0, 3, 1, 1, 1, 0, 1, 2, color2));
            Triangles.Add(new Triangle(4, 3, 7, 1, 1, 1, 0, 2, 3, color2));
            Triangles.Add(new Triangle(5, 4, 7, 2, 2, 2, 0, 1, 2, color3));
            Triangles.Add(new Triangle(5, 7, 6, 2, 2, 2, 0, 2, 3, color3));
            Triangles.Add(new Triangle(1, 5, 6, 3, 3, 3, 0, 1, 2, color4));
            Triangles.Add(new Triangle(1, 6, 2, 3, 3, 3, 0, 2, 3, color4));
            Triangles.Add(new Triangle(4, 5, 1, 4, 4, 4, 0, 1, 2, color5));
            Triangles.Add(new Triangle(4, 1, 0, 4, 4, 4, 0, 2, 3, color5));
            Triangles.Add(new Triangle(2, 6, 7, 5, 5, 5, 0, 1, 2, color6));
            Triangles.Add(new Triangle(2, 7, 3, 5, 5, 5, 0, 2, 3, color6));

            CalculateBoundingSphere();
        }
    }
}
