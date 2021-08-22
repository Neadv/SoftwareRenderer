using System;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class SoftwareRasterizer : IRenderer
    {
        private ICanvas _canvas;

        public void Initialization()
        {

        }

        public void Render(ICanvas canvas)
        {
            _canvas = canvas;

            DrawLine(new Vector2i(-200, -100), new Vector2i(200, 100), Color.Black);
            DrawLine(new Vector2i(-50, -200), new Vector2i(60, 240), Color.Black);
        }

        private void DrawLine(Vector2i p0, Vector2i p1, Color color)
        {
            if (Math.Abs(p1.X - p0.X) > Math.Abs(p1.Y - p0.Y))
            {
                if (p0.X > p1.X)
                {
                    Swap(ref p0, ref p1);
                }
                var ys = Interpolate(p0.X, p0.Y, p1.X, p1.Y);
                for (int x = p0.X; x <= p1.X; x++)
                {
                    _canvas.SetColor(x, (int)ys[x - p0.X], color);
                }
            }
            else
            {
                if (p0.Y > p1.Y)
                {
                    Swap(ref p0, ref p1);
                }
                var xs = Interpolate(p0.Y, p0.X, p1.Y, p1.X);
                for (int y = p0.Y; y <= p1.Y; y++)
                {
                    _canvas.SetColor((int)xs[y - p0.Y], y, color);
                }
            }
        }

        private float[] Interpolate(int i0, int d0, int i1, int d1)
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

        private void Swap<T>(ref T obj1, ref T obj2)
        {
            T tmp = obj1;
            obj1 = obj2;
            obj2 = tmp;
        }
    }
}