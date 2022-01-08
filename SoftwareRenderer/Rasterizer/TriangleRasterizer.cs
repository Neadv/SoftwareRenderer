using SoftwareRenderer.Common;
using SoftwareRenderer.Utils;
using System;

namespace SoftwareRenderer.Rasterizer
{
    public class TriangleRasterizer
    {
        private readonly ICanvas _canvas;
        private readonly ZBuffer _zBuffer;

        public TriangleRasterizer(ICanvas canvas, ZBuffer zBuffer)
        {
            _canvas = canvas;
            _zBuffer = zBuffer;
        }

        public void DrawWireframeTriangle(Vector2i p0, Vector2i p1, Vector2i p2, Color color)
        {
            DrawLine(p0, p1, color);
            DrawLine(p1, p2, color);
            DrawLine(p0, p2, color);
        }

        public void DrawFilledTriangle(Vector2i p0,
                                        Vector2i p1,
                                        Vector2i p2,
                                        float[] zPositions,
                                        Color color)
        {
            // Sort the points 
            if (p1.Y < p0.Y)
            {
                MathHelper.Swap(ref p1, ref p0);
                MathHelper.Swap(ref zPositions[1], ref zPositions[0]);
            }
            if (p2.Y < p0.Y)
            {
                MathHelper.Swap(ref p2, ref p0);
                MathHelper.Swap(ref zPositions[2], ref zPositions[0]);
            }
            if (p2.Y < p1.Y)
            {
                MathHelper.Swap(ref p2, ref p1);
                MathHelper.Swap(ref zPositions[2], ref zPositions[1]);
            }

            // Compute the x coordinates of the triangles edges
            float[] x01 = MathHelper.Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            float[] x12 = MathHelper.Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            float[] x02 = MathHelper.Interpolate(p0.Y, p0.X, p2.Y, p2.X);

            float[] z01 = MathHelper.Interpolate(p0.Y, 1 / zPositions[0], p1.Y, 1 / zPositions[1]);
            float[] z12 = MathHelper.Interpolate(p1.Y, 1 / zPositions[1], p2.Y, 1 / zPositions[2]);
            float[] z02 = MathHelper.Interpolate(p0.Y, 1 / zPositions[0], p2.Y, 1 / zPositions[2]);

            // Concatenate the short sides
            float[] x012 = new float[x02.Length];
            Array.Copy(x01, x012, x01.Length - 1);
            Array.Copy(x12, 0, x012, x01.Length - 1, x12.Length);

            float[] z012 = new float[z02.Length];
            Array.Copy(z01, z012, z01.Length - 1);
            Array.Copy(z12, 0, z012, z01.Length - 1, z12.Length);

            // Determine which is left and which is right
            var m = x012.Length / 2;
            float[] x_left, x_right, z_left, z_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                z_left = z02;
                z_right = z012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                z_left = z012;
                z_right = z02;
            }

            // Draw the horizontal segments
            for (int y = p0.Y; y < p2.Y; y++)
            {
                int x_l = (int)x_left[y - p0.Y];
                int x_r = (int)x_right[y - p0.Y];

                float[] z_segment = MathHelper.Interpolate(x_l, z_left[y - p0.Y], x_r, z_right[y - p0.Y]);
                for (int x = x_l; x < x_r; x++)
                {
                    float z = z_segment[x - x_l];
                    if (z > _zBuffer.Get(x, y))
                    {
                        _canvas.Set(x, y, color);
                        _zBuffer.Set(x, y, z);
                    }
                }
            }
        }

        public void DrawShadedTriangle(Vector2i p0, Vector2i p1, Vector2i p2, float[] attrs, float[] zPositions, Color color)
        {
            // Sort the points 
            if (p1.Y < p0.Y)
            {
                MathHelper.Swap(ref p1, ref p0);
                MathHelper.Swap(ref attrs[1], ref attrs[0]);
            }
            if (p2.Y < p0.Y)
            {
                MathHelper.Swap(ref p2, ref p0);
                MathHelper.Swap(ref attrs[2], ref attrs[0]);
            }
            if (p2.Y < p1.Y)
            {
                MathHelper.Swap(ref p2, ref p1);
                MathHelper.Swap(ref attrs[2], ref attrs[1]);
            }

            // Compute the x coordinates and h of the triangles edges
            float[] x01 = MathHelper.Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            float[] h01 = MathHelper.Interpolate(p0.Y, attrs[0], p1.Y, attrs[1]);
            float[] z01 = MathHelper.Interpolate(p0.Y, 1 / zPositions[0], p1.Y, 1 / zPositions[1]);

            float[] x12 = MathHelper.Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            float[] h12 = MathHelper.Interpolate(p1.Y, attrs[1], p2.Y, attrs[2]);
            float[] z12 = MathHelper.Interpolate(p1.Y, 1 / zPositions[1], p2.Y, 1 / zPositions[2]);

            float[] x02 = MathHelper.Interpolate(p0.Y, p0.X, p2.Y, p2.X);
            float[] h02 = MathHelper.Interpolate(p0.Y, attrs[0], p2.Y, attrs[2]);
            float[] z02 = MathHelper.Interpolate(p0.Y, 1 / zPositions[0], p2.Y, 1 / zPositions[2]);

            // Concatenate the short sides
            var x012 = new float[x02.Length];
            Array.Copy(x01, x012, x01.Length - 1);
            Array.Copy(x12, 0, x012, x01.Length - 1, x12.Length);

            var h012 = new float[h02.Length];
            Array.Copy(h01, h012, h01.Length - 1);
            Array.Copy(h12, 0, h012, h01.Length - 1, h12.Length);

            float[] z012 = new float[z02.Length];
            Array.Copy(z01, z012, z01.Length - 1);
            Array.Copy(z12, 0, z012, z01.Length - 1, z12.Length);

            // Determine which is left and which is right
            var m = x012.Length / 2;
            float[] x_left, x_right, h_left, h_right, z_left, z_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                h_left = h02;
                h_right = h012;

                z_left = z02;
                z_right = z012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                h_left = h012;
                h_right = h02;

                z_left = z012;
                z_right = z02;
            }

            // Draw the horizontal segments
            for (int y = p0.Y; y < p2.Y; y++)
            {
                int x_l = (int)x_left[y - p0.Y];
                int x_r = (int)x_right[y - p0.Y];

                float[] h_segment = MathHelper.Interpolate(x_l, h_left[y - p0.Y], x_r, h_right[y - p0.Y]);
                float[] z_segment = MathHelper.Interpolate(x_l, z_left[y - p0.Y], x_r, z_right[y - p0.Y]);
                for (int x = x_l; x < x_r; x++)
                {
                    float z = z_segment[x - x_l];
                    if (z > _zBuffer.Get(x, y))
                    {
                        var shadedColor = h_segment[x - x_l] * color;
                        _canvas.Set(x, y, shadedColor);
                        _zBuffer.Set(x, y, z);
                    }
                }
            }
        }

        private void DrawLine(Vector2i p0, Vector2i p1, Color color)
        {
            if (Math.Abs(p1.X - p0.X) > Math.Abs(p1.Y - p0.Y))
            {
                if (p0.X > p1.X)
                {
                    MathHelper.Swap(ref p0, ref p1);
                }
                var ys = MathHelper.Interpolate(p0.X, p0.Y, p1.X, p1.Y);
                for (int x = p0.X; x <= p1.X; x++)
                {
                    _canvas.Set(x, (int)ys[x - p0.X], color);
                }
            }
            else
            {
                if (p0.Y > p1.Y)
                {
                    MathHelper.Swap(ref p0, ref p1);
                }
                var xs = MathHelper.Interpolate(p0.Y, p0.X, p1.Y, p1.X);
                for (int y = p0.Y; y <= p1.Y; y++)
                {
                    _canvas.Set((int)xs[y - p0.Y], y, color);
                }
            }
        }
    }
}
