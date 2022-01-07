using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class Cube : Mesh
    {
        public Cube(float size, Color color) 
            : this(size, color, color, color, color, color, color)
        { }

        public Cube(float size, Color color1, Color color2, Color color3, Color color4, Color color5, Color color6)
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

            Triangles.Add(new Triangle(0, 1, 2, color1));
            Triangles.Add(new Triangle(0, 2, 3, color1));
            Triangles.Add(new Triangle(4, 0, 3, color2));
            Triangles.Add(new Triangle(4, 3, 7, color2));
            Triangles.Add(new Triangle(5, 4, 7, color3));
            Triangles.Add(new Triangle(5, 7, 6, color3));
            Triangles.Add(new Triangle(1, 5, 6, color4));
            Triangles.Add(new Triangle(1, 6, 2, color4));
            Triangles.Add(new Triangle(4, 5, 1, color5));
            Triangles.Add(new Triangle(4, 1, 0, color5));
            Triangles.Add(new Triangle(2, 6, 7, color6));
            Triangles.Add(new Triangle(2, 7, 3, color6));

            CalculateBoundingSphere();
        }
    }
}
