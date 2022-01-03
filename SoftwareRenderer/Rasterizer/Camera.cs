using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class Camera
    {
        public Vector3f Position { get; set; }
        public Matrix4x4 Orientation { get; set; }

        public Camera(Vector3f position, Matrix4x4 orientation)
        {
            Position = position;
            Orientation = orientation;
        }

        public Camera()
        {
            Position = new Vector3f(0);
            Orientation = Matrix4x4.Identity;
        }

    }
}
