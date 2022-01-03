﻿using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class Instance
    {
        public Mesh Mesh { get; }
        public Matrix4x4 Transform { get; }

        public float Scale { get; }
        public Matrix4x4 Orientation { get; }
        public Vector3f Position { get; }

        public Instance(Mesh mesh, Vector3f position, Matrix4x4 orientation, float scale)
        {
            Mesh = mesh;

            Orientation = orientation;
            Position = position;
            Scale = scale;

            Transform = TransformHelper.MakeTranslationMatrix(position) * (orientation * TransformHelper.MakeScalingMatrix(scale));
        }

        public Instance(Mesh mesh, Vector3f position)
            : this(mesh, position, Matrix4x4.Identity, 1)
        { }

        public Instance(Mesh mesh, Vector3f position, float scale)
            : this(mesh, position, Matrix4x4.Identity, scale)
        { }

        public Instance(Mesh mesh)
            : this(mesh, new Vector3f(0), Matrix4x4.Identity, 1)
        { }
    }
}