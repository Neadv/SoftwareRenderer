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

        public static void Swap<T>(ref T obj1, ref T obj2)
        {
            T tmp = obj1;
            obj1 = obj2;
            obj2 = tmp;
        }
    }
}
