using SoftwareRenderer.Common;
using SoftwareRenderer.Utils;
using System;
using System.Collections.Generic;

namespace SoftwareRenderer.Rasterizer
{
    public class TriangleRasterizer
    {
        private readonly ICanvas _canvas;
        private readonly ZBuffer _zBuffer;
        private readonly Camera _camera;
        private readonly Viewport _viewport;

        public TriangleRasterizer(ICanvas canvas, ZBuffer zBuffer, Camera camera, Viewport viewport)
        {
            _canvas = canvas;
            _zBuffer = zBuffer;
            _camera = camera;
            _viewport = viewport;
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
            var (x02, x012) = EdgeInterpolate(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            var (z02, z012) = EdgeInterpolate(p0.Y, 1 / zPositions[0], p1.Y, 1 / zPositions[1], p2.Y, 1 / zPositions[2]);

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

        public void DrawShadedTriangle(Vector2i p0,
                                       Vector2i p1,
                                       Vector2i p2,
                                       float[] zPositions,
                                       Vector3f[] normals,
                                       IEnumerable<Light> lights,
                                       Matrix4x4 orientation,
                                       Color color)
        {
            // Sort the points 
            if (p1.Y < p0.Y)
            {
                MathHelper.Swap(ref p1, ref p0);
                MathHelper.Swap(ref zPositions[1], ref zPositions[0]);
                MathHelper.Swap(ref normals[1], ref normals[0]);
            }
            if (p2.Y < p0.Y)
            {
                MathHelper.Swap(ref p2, ref p0);
                MathHelper.Swap(ref zPositions[2], ref zPositions[0]);
                MathHelper.Swap(ref normals[2], ref normals[0]);
            }
            if (p2.Y < p1.Y)
            {
                MathHelper.Swap(ref p2, ref p1);
                MathHelper.Swap(ref zPositions[2], ref zPositions[1]);
                MathHelper.Swap(ref normals[2], ref normals[1]);
            }

            // Compute the x coordinates and h of the triangles edges
            var (x02, x012) = EdgeInterpolate(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            var (z02, z012) = EdgeInterpolate(p0.Y, 1 / zPositions[0], p1.Y, 1 / zPositions[1], p2.Y, 1 / zPositions[2]);

            // Shading
            var transform = _camera.Orientation.Transpose() * orientation;
            normals[0] = transform * normals[0];
            normals[1] = transform * normals[1];
            normals[2] = transform * normals[2];

            var (nx02, nx012) = EdgeInterpolate(p0.Y, normals[0].X, p1.Y, normals[1].X, p2.Y, normals[2].X);
            var (ny02, ny012) = EdgeInterpolate(p0.Y, normals[0].Y, p1.Y, normals[1].Y, p2.Y, normals[2].Y);
            var (nz02, nz012) = EdgeInterpolate(p0.Y, normals[0].Z, p1.Y, normals[1].Z, p2.Y, normals[2].Z);


            // Determine which is left and which is right
            var m = x012.Length / 2;
            float[] x_left, x_right, z_left, z_right, nx_left, nx_right, ny_left, ny_right, nz_left, nz_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                z_left = z02;
                z_right = z012;

                nx_left = nx02;
                nx_right = nx012;

                ny_left = ny02;
                ny_right = ny012;

                nz_left = nz02;
                nz_right = nz012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                z_left = z012;
                z_right = z02;

                nx_left = nx012;
                nx_right = nx02;

                ny_left = ny012;
                ny_right = ny02;

                nz_left = nz012;
                nz_right = nz02;
            }

            // Draw the horizontal segments
            for (int y = p0.Y; y < p2.Y; y++)
            {
                int x_l = (int)x_left[y - p0.Y];
                int x_r = (int)x_right[y - p0.Y];

                float[] z_segment = MathHelper.Interpolate(x_l, z_left[y - p0.Y], x_r, z_right[y - p0.Y]);

                float[] nx_segment = MathHelper.Interpolate(x_l, nx_left[y - p0.Y], x_r, nx_right[y - p0.Y]);
                float[] ny_segment = MathHelper.Interpolate(x_l, ny_left[y - p0.Y], x_r, ny_right[y - p0.Y]);
                float[] nz_segment = MathHelper.Interpolate(x_l, nz_left[y - p0.Y], x_r, nz_right[y - p0.Y]);
                for (int x = x_l; x < x_r; x++)
                {
                    float z = z_segment[x - x_l];
                    if (z > _zBuffer.Get(x, y))
                    {
                        Vector3f normal = new Vector3f(nx_segment[x - x_l], ny_segment[x - x_l], nz_segment[x - x_l]);
                        Vector3f vertex = UnProjectVertex(x, y, z);
                        float intensity = IlluminationCalculator.ComputeIllumination(vertex, normal, _camera, lights);

                        _canvas.Set(x, y, color * intensity);
                        _zBuffer.Set(x, y, z);
                    }
                }
            }
        }

        public void DrawTexturedTriangle(Vector2i p0,
                                       Vector2i p1,
                                       Vector2i p2,
                                       float[] zPositions,
                                       Vector2f[] uvs,
                                       Image texture,
                                       Vector3f[] normals,
                                       IEnumerable<Light> lights,
                                       Matrix4x4 orientation)
        {
            // Sort the points 
            if (p1.Y < p0.Y)
            {
                MathHelper.Swap(ref p1, ref p0);
                MathHelper.Swap(ref zPositions[1], ref zPositions[0]);
                MathHelper.Swap(ref normals[1], ref normals[0]);
                MathHelper.Swap(ref uvs[1], ref uvs[0]);
            }
            if (p2.Y < p0.Y)
            {
                MathHelper.Swap(ref p2, ref p0);
                MathHelper.Swap(ref zPositions[2], ref zPositions[0]);
                MathHelper.Swap(ref normals[2], ref normals[0]);
                MathHelper.Swap(ref uvs[2], ref uvs[0]);
            }
            if (p2.Y < p1.Y)
            {
                MathHelper.Swap(ref p2, ref p1);
                MathHelper.Swap(ref zPositions[2], ref zPositions[1]);
                MathHelper.Swap(ref normals[2], ref normals[1]);
                MathHelper.Swap(ref uvs[2], ref uvs[1]);
            }

            // Compute the x coordinates and h of the triangles edges
            var (x02, x012) = EdgeInterpolate(p0.Y, p0.X, p1.Y, p1.X, p2.Y, p2.X);
            var (z02, z012) = EdgeInterpolate(p0.Y, 1 / zPositions[0], p1.Y, 1 / zPositions[1], p2.Y, 1 / zPositions[2]);

            // Shading
            var transform = _camera.Orientation.Transpose() * orientation;
            normals[0] = transform * normals[0];
            normals[1] = transform * normals[1];
            normals[2] = transform * normals[2];

            var (nx02, nx012) = EdgeInterpolate(p0.Y, normals[0].X, p1.Y, normals[1].X, p2.Y, normals[2].X);
            var (ny02, ny012) = EdgeInterpolate(p0.Y, normals[0].Y, p1.Y, normals[1].Y, p2.Y, normals[2].Y);
            var (nz02, nz012) = EdgeInterpolate(p0.Y, normals[0].Z, p1.Y, normals[1].Z, p2.Y, normals[2].Z);

            // Texturing
            var uv0 = uvs[0] / zPositions[0];
            var uv1 = uvs[1] / zPositions[1];
            var uv2 = uvs[2] / zPositions[2];
            var (uvx02, uvx012) = EdgeInterpolate(p0.Y, uv0.X, p1.Y, uv1.X, p2.Y, uv2.X);
            var (uvy02, uvy012) = EdgeInterpolate(p0.Y, uv0.Y, p1.Y, uv1.Y, p2.Y, uv2.Y);


            // Determine which is left and which is right
            var m = x012.Length / 2;
            float[] x_left, x_right, z_left, z_right, nx_left, nx_right, ny_left, ny_right, nz_left, nz_right, uvx_left, uvx_right, uvy_left, uvy_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                z_left = z02;
                z_right = z012;

                nx_left = nx02;
                nx_right = nx012;

                ny_left = ny02;
                ny_right = ny012;

                nz_left = nz02;
                nz_right = nz012;

                uvx_left = uvx02;
                uvx_right = uvx012;

                uvy_left = uvy02;
                uvy_right = uvy012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                z_left = z012;
                z_right = z02;

                nx_left = nx012;
                nx_right = nx02;

                ny_left = ny012;
                ny_right = ny02;

                nz_left = nz012;
                nz_right = nz02;

                uvx_left = uvx012;
                uvx_right = uvx02;

                uvy_left = uvy012;
                uvy_right = uvy02;
            }

            // Draw the horizontal segments
            if (p2.Y - p0.Y == 1)
            {

            }
            for (int y = p0.Y; y <= p2.Y; y++)
            {
                int x_l = (int)x_left[y - p0.Y];
                int x_r = (int)x_right[y - p0.Y];

                float[] z_segment = MathHelper.Interpolate(x_l, z_left[y - p0.Y], x_r, z_right[y - p0.Y]);

                // Shading
                float[] nx_segment = MathHelper.Interpolate(x_l, nx_left[y - p0.Y], x_r, nx_right[y - p0.Y]);
                float[] ny_segment = MathHelper.Interpolate(x_l, ny_left[y - p0.Y], x_r, ny_right[y - p0.Y]);
                float[] nz_segment = MathHelper.Interpolate(x_l, nz_left[y - p0.Y], x_r, nz_right[y - p0.Y]);

                // Texturing
                float[] uvx_segment = MathHelper.Interpolate(x_l, uvx_left[y - p0.Y], x_r, uvx_right[y - p0.Y]);
                float[] uvy_segment = MathHelper.Interpolate(x_l, uvy_left[y - p0.Y], x_r, uvy_right[y - p0.Y]);

                for (int x = x_l; x <= x_r; x++)
                {
                    float z = z_segment[x - x_l];
                    if (z > _zBuffer.Get(x, y))
                    {
                        Vector3f normal = new Vector3f(nx_segment[x - x_l], ny_segment[x - x_l], nz_segment[x - x_l]);
                        Vector3f vertex = UnProjectVertex(x, y, z);
                        float intensity = IlluminationCalculator.ComputeIllumination(vertex, normal, _camera, lights);

                        Vector2f uv = new Vector2f(uvx_segment[x - x_l] / z_segment[x - x_l], uvy_segment[x - x_l] / z_segment[x - x_l]);
                        Color texel = texture.Get(uv);

                        _canvas.Set(x, y, texel * intensity);
                        _zBuffer.Set(x, y, z);
                    }
                }
            }
        }

        private static (float[], float[]) EdgeInterpolate(int f0, float t0, int f1, float t1, int f2, float t2)
        {
            float[] v01 = MathHelper.Interpolate(f0, t0, f1, t1);
            float[] v12 = MathHelper.Interpolate(f1, t1, f2, t2);
            float[] v02 = MathHelper.Interpolate(f0, t0, f2, t2);

            float[] v012 = new float[v02.Length];
            Array.Copy(v01, v012, v01.Length - 1);
            Array.Copy(v12, 0, v012, v01.Length - 1, v12.Length);

            return (v02, v012);
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
        private Vector3f UnProjectVertex(int x, int y, float inv_z)
        {
            var oz = 1.0f / inv_z;
            var ux = x * oz / _viewport.Distance;
            var uy = y * oz / _viewport.Distance;
            return new Vector3f(ux * _viewport.Width / _canvas.Width, uy * _viewport.Height / _canvas.Height, oz);
        }
    }
}
