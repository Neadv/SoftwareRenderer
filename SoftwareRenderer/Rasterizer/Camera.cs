using SoftwareRenderer.Common;
using System;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class Camera
    {
        public Vector3f Position { get; set; }
        public Matrix4x4 Orientation { get; set; }
        public List<Plane> ClippingPlanes { get; set; }

        public Camera(Vector3f position, Matrix4x4 orientation)
        {
            Position = position;
            Orientation = orientation;

            ClippingPlanes = new List<Plane>
            {
                new Plane(new Vector3f(0, 0, 1), -1),                           // Near
                new Plane(new Vector3f(MathF.Sqrt(2), 0, MathF.Sqrt(2)), 0),    // Left
                new Plane(new Vector3f(-MathF.Sqrt(2), 0, MathF.Sqrt(2)), 0),   // Right
                new Plane(new Vector3f(0, -MathF.Sqrt(2), MathF.Sqrt(2)), 0),   // Top
                new Plane(new Vector3f(0, MathF.Sqrt(2), MathF.Sqrt(2)), 0)     // Bottom
            };
        }

        public Camera()
            : this(new Vector3f(0), Matrix4x4.Identity)
        { }

    }
}
