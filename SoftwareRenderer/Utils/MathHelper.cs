using SoftwareRenderer.Common;
using System;
using System.Collections.Generic;

namespace SoftwareRenderer.Utils
{
    public static class MathHelper
    {
        public static float[] Interpolate(int i0, float d0, int i1, float d1)
        {
            if (i0 == i1)
            {
                return new float[] { d0 };
            }
            float[] values = new float[i1 - i0 + 1];
            float a = (float)(d1 - d0) / (i1 - i0);
            float d = d0;
            for (int i = i0; i <= i1; i++)
            {
                values[i - i0] = d;
                d += a;
            }
            return values;
        }

        internal static Sphere CalculateBoundingSphere(List<Vector3f> vertices)
        {
            Vector3f center = new Vector3f(0);
            foreach (var vertex in vertices)
            {
                center += vertex;
            }
            center /= vertices.Count;

            float radius = float.MinValue;
            foreach (var vertex in vertices)
            {
                float length = (vertex - center).Length();
                if (length > radius)
                {
                    radius = length;
                }
            }
            return new Sphere(center, radius);
        }

        public static void Swap<T>(ref T obj1, ref T obj2)
        {
            T tmp = obj1;
            obj1 = obj2;
            obj2 = tmp;
        }

        public static Vector3f ComputeTriangleNormal(Vector3f v0, Vector3f v1, Vector3f v2)
        {
            var v0v1 = v1 - v0;
            var v0v2 = v2 - v0;
            return v0v1.Cross(v0v2).Normalize();
        }
    }
}
