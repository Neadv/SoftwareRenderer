using System;
using System.Collections.Generic;
using SoftwareRenderer.Common;
using SoftwareRenderer.Utils;

namespace SoftwareRenderer.Rasterizer
{
    public class SoftwareRasterizer : IRenderer
    {
        private ICanvas _canvas;
        private Camera _camera;
        private Viewport _viewport;

        public void Initialization()
        {
            _camera = new Camera();
            _viewport = new Viewport(1, 1, 1);
        }

        public void Render(ICanvas canvas)
        {
            _canvas = canvas;

            _camera.Position = new Vector3f(-3, 1, 2);
            _camera.Orientation = TransformHelper.MakeOYRotationMatrix(-30);

            Scene scene = new Scene();

            Mesh cube = new Cube(2, Color.Red, Color.Green, Color.Blue, Color.Red, Color.Green, Color.Blue);

            scene.Instances.Add(new Instance(cube, new Vector3f(-1.5f, 0, 7), 0.75f));
            scene.Instances.Add(new Instance(cube, new Vector3f(1.25f, 2.5f, 7.5f), TransformHelper.MakeOYRotationMatrix(195), 1));

            RenderScene(scene);
        }

        public void RenderScene(Scene scene)
        {
            var cameraMatrix = _camera.Orientation.Transpose() * TransformHelper.MakeTranslationMatrix(-_camera.Position);

            foreach (var instance in scene.Instances)
            {
                var transform = cameraMatrix * instance.Transform;
                RenderModel(instance, transform);
            }
        }

        private void RenderModel(Instance instance, Matrix4x4 transform)
        {
            var mesh = instance.Mesh;
            var projected = new Vector2i[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var vertex = mesh.Vertices[i];
                var vert4 = new Vector4f(vertex.X, vertex.Y, vertex.Z, 1);
                var transformed = transform * vert4;
                projected[i] = PointToCanvas(new Vector3f(transformed.X, transformed.Y, transformed.Z));
            }
            foreach (var triangle in mesh.Triangles)
            {
                RenderTriangle(triangle, projected);
            }
        }

        private void RenderTriangle(Triangle triangle, Vector2i[] projected)
        {
            DrawWireframeTriangle(projected[triangle.V1],
                                  projected[triangle.V2],
                                  projected[triangle.V3],
                                  triangle.Color);
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
                    _canvas.SetColor(x, (int)ys[x - p0.X], color);
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
            if (p1.Y < p0.Y) MathHelper.Swap(ref p1, ref p0);
            if (p2.Y < p0.Y) MathHelper.Swap(ref p2, ref p0);
            if (p2.Y < p1.Y) MathHelper.Swap(ref p2, ref p1);

            // Compute the x coordinates of the triangles edges
            var x01 = MathHelper.Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            var x12 = MathHelper.Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            var x02 = MathHelper.Interpolate(p0.Y, p0.X, p2.Y, p2.X);

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

        private void DrawShadedTriangle(Vector2i p0, Vector2i p1, Vector2i p2, float[] attrs, Color color)
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
            var x01 = MathHelper.Interpolate(p0.Y, p0.X, p1.Y, p1.X);
            var h01 = MathHelper.Interpolate(p0.Y, attrs[0], p1.Y, attrs[1]);

            var x12 = MathHelper.Interpolate(p1.Y, p1.X, p2.Y, p2.X);
            var h12 = MathHelper.Interpolate(p1.Y, attrs[1], p2.Y, attrs[2]);

            var x02 = MathHelper.Interpolate(p0.Y, p0.X, p2.Y, p2.X);
            var h02 = MathHelper.Interpolate(p0.Y, attrs[0], p2.Y, attrs[2]);

            // Concatenate the short sides
            var x012 = new float[x02.Length];
            Array.Copy(x01, x012, x01.Length - 1);
            Array.Copy(x12, 0, x012, x01.Length - 1, x12.Length);

            var h012 = new float[h02.Length];
            Array.Copy(h01, h012, h01.Length - 1);
            Array.Copy(h12, 0, h012, h01.Length - 1, h12.Length);

            // Determine which is left and which is right
            var m = x012.Length / 2;
            float[] x_left, x_right, h_left, h_right;
            if (x02[m] < x012[m])
            {
                x_left = x02;
                x_right = x012;

                h_left = h02;
                h_right = h012;
            }
            else
            {
                x_left = x012;
                x_right = x02;

                h_left = h012;
                h_right = h02;
            }

            // Draw the horizontal segments
            for (int y = p0.Y; y < p2.Y; y++)
            {
                int x_l = (int)x_left[y - p0.Y];
                int x_r = (int)x_right[y - p0.Y];

                float[] h_segment = MathHelper.Interpolate(x_l, h_left[y - p0.Y], x_r, h_right[y - p0.Y]);
                for (int x = x_l; x < x_r; x++)
                {
                    var shadedColor = h_segment[x - x_l] * color;
                    _canvas.SetColor(x, y, shadedColor);
                }
            }
        }

        private Vector2i PointToCanvas(Common.Vector3f point)
        {
            var viewportPoint = PointToViewport(point);
            int x = (int)((viewportPoint.X * _canvas.Width) / _viewport.Width);
            int y = (int)((viewportPoint.Y * _canvas.Height) / _viewport.Height);
            return new Vector2i(x, y);
        }

        private Common.Vector3f PointToViewport(Common.Vector3f point)
        {
            var x = (point.X * _viewport.Distance) / point.Z;
            var y = (point.Y * _viewport.Distance) / point.Z;
            return new Vector3f(x, y, _viewport.Distance);
        }
    }
}