using SoftwareRenderer.Common;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer.SceneBuilder
{
    public class CameraOptionsBuilder
    {
        public Camera Camera { get; private set; } = new Camera();

        public CameraOptionsBuilder SetPosition(Vector3f position)
        {
            Camera.Position = position;
            return this;
        }

        public CameraOptionsBuilder SetOrientation(Matrix4x4 orientation)
        {
            Camera.Orientation = orientation;
            return this;
        }

        public CameraOptionsBuilder SetClippingPlanes(List<Plane> planes)
        {
            Camera.ClippingPlanes = planes;
            return this;
        }
    }
}
