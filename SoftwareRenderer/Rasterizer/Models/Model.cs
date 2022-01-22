using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer.Models
{
    public class Model
    {
        public Mesh Mesh { get; }
        public Matrix4x4 Transform { get; }
        public Image Texture { get; set; }

        public float Scale { get; }
        public Matrix4x4 Orientation { get; }
        public Vector3f Position { get; }


        public Model(Mesh mesh, Vector3f position, Matrix4x4 orientation, float scale)
        {
            Mesh = mesh;

            Orientation = orientation;
            Position = position;
            Scale = scale;

            Transform = TransformHelper.MakeTranslationMatrix(position) * (orientation * TransformHelper.MakeScalingMatrix(scale));
        }

        public Model(Mesh mesh, Vector3f position)
            : this(mesh, position, Matrix4x4.Identity, 1)
        { }

        public Model(Mesh mesh, Vector3f position, float scale)
            : this(mesh, position, Matrix4x4.Identity, scale)
        { }

        public Model(Mesh mesh)
        {
            Mesh = mesh;

            Orientation = Matrix4x4.Identity;
            Position = new Vector3f(0);
            Scale = 1;

            Transform = Matrix4x4.Identity;
        }
    }
}
