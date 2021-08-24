using System;
using System.Collections.Generic;
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

            DrawFilledTriangle(new Vector2i(-200, -250), new Vector2i(200, 50), new Vector2i(20, 250), Color.Green);
            DrawWireframeTriangle(new Vector2i(-200, -250), new Vector2i(200, 50), new Vector2i(20, 250), Color.Black);
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

        private void DrawWireframeTriangle(Vector2i p0, Vector2i p1, Vector2i p2, Color color)
        {
            DrawLine(p0, p1, color);
            DrawLine(p1, p2, color);
            DrawLine(p0, p2, color);
        }

        private void DrawFilledTriangle(Vector2i p0, Vector2i p1, Vector2i p2, Color color)
        {
            // Sort the points 
            if (p1.Y < p0.Y) Swap(ref p1, ref p0);
            if (p2.Y < p0.Y) Swap(ref p2, ref p0);
            if (p2.Y < p1.Y) Swap(ref p2, ref p1);

            // Compute the x coordinates of the triangles edges
            var x01 = Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            var x12 = Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            var x02 = Interpolate(p0.Y, p0.X, p2.Y, p2.X);

            // Concatenate the short sides
            var x012 = new float[x02.Length];
            Array.Copy(x01, x012, x01.Length - 1);
            Array.Copy(x12, 0, x012, x01.Length - 1, x12.Length);

            // Determine which is left and which is right
            var m = x012.Length / 2;
            float[] x_left;
            float[] x_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;
            }
            else
            {
                x_left = x012;
                x_right = x02;
            }

            // Draw the horizontal segments
            for (int y = p0.Y; y < p2.Y; y++)
            {
                for (int x = (int)x_left[y - p0.Y]; x < x_right[y - p0.Y]; x++)
                {
                    _canvas.SetColor(x, y, color);
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