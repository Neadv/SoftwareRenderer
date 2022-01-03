using SoftwareRenderer.Common;
using System;

namespace SoftwareRenderer.Rasterizer
{
    public static class TransformHelper
    {
        public static Matrix4x4 MakeScalingMatrix(float scale)
        {
            return new Matrix4x4(new Vector4f(scale, 0, 0, 0),
                                 new Vector4f(0, scale, 0, 0),
                                 new Vector4f(0, 0, scale, 0),
                                 new Vector4f(0, 0, 0, 1));
        }

        public static Matrix4x4 MakeTranslationMatrix(Vector3f translate)
        {
            return new Matrix4x4(new Vector4f(1, 0, 0, translate.X),
                                 new Vector4f(0, 1, 0, translate.Y),
                                 new Vector4f(0, 0, 1, translate.Z),
                                 new Vector4f(0, 0, 0, 1));
        }

        public static Matrix4x4 MakeOYRotationMatrix(float degrees)
        {
            float cos = MathF.Cos(degrees * MathF.PI / 180.0f);
            float sin = MathF.Sin(degrees * MathF.PI / 180.0f);

            return new Matrix4x4(new Vector4f(cos, 0, -sin, 0),
                                 new Vector4f(0, 1, 0, 0),
                                 new Vector4f(sin, 0, cos, 0),
                                 new Vector4f(0, 0, 0, 1));
        }
    }
}
