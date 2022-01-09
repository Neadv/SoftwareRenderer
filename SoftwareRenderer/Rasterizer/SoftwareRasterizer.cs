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
        private ZBuffer _zBuffer;

        private TriangleRasterizer _triangleRasterizer;

        private int _trianglesRendered = 0;

        public void Initialization(ICanvas canvas)
        {
            _canvas = canvas;
            _zBuffer = new ZBuffer(_canvas.Width, _canvas.Height);

            _camera = new Camera();
            _viewport = new Viewport(1, 1, 1);

            _camera.Position = new Vector3f(-3, 1, 2);
            _camera.Orientation = TransformHelper.MakeOYRotationMatrix(-30);

            _triangleRasterizer = new TriangleRasterizer(_canvas, _zBuffer, _camera, _viewport);
        }

        public void Render()
        {
            Scene scene = new Scene();

            Mesh cube = new CubeMesh(2, Color.Red, Color.Green, Color.Blue, Color.Red, Color.Green, Color.Blue);
            Mesh sphere = new SphereMesh(1, Color.Green);

            scene.Instances.Add(new Instance(cube, new Vector3f(-1.5f, 0, 7), 0.75f));
            scene.Instances.Add(new Instance(cube, new Vector3f(1.25f, 2.5f, 7.5f), TransformHelper.MakeOYRotationMatrix(195), 1));
            scene.Instances.Add(new Instance(cube, new Vector3f(10, 0, 10)));
            scene.Instances.Add(new Instance(cube, new Vector3f(-10, 0, -10), TransformHelper.MakeOYRotationMatrix(195), 1));
            scene.Instances.Add(new Instance(sphere, new Vector3f(1.75f, -0.5f, 7), Matrix4x4.Identity, 1.5f));

            scene.Lights.Add(Light.CreateAmbient(0.2f));
            scene.Lights.Add(Light.CreateDirectional(0.2f, new Vector3f(-1, 0, 1).Normalize()));
            scene.Lights.Add(Light.CreatePoint(0.6f, new Vector3f(-3, 3, -10)));

            RenderScene(scene);
            System.Console.WriteLine($"Triangles Rendered: {_trianglesRendered}");
        }

        public void RenderScene(Scene scene)
        {
            var cameraMatrix = _camera.Orientation.Transpose() * TransformHelper.MakeTranslationMatrix(-_camera.Position);

            foreach (var instance in scene.Instances)
            {
                var transformed = MeshTransformation.TransformAndClip(instance.Mesh, cameraMatrix * instance.Transform, _camera.ClippingPlanes);
                if (transformed != null)
                {
                    RenderModel(transformed, scene.Lights, instance.Orientation);
                }
            }
        }

        private void RenderModel(Mesh mesh, IEnumerable<Light> lights, Matrix4x4 orientation)
        {
            foreach (var triangle in mesh.Triangles)
            {
                RenderTriangle(triangle, mesh.Vertices, lights, orientation);
            }
        }

        private void RenderTriangle(Triangle triangle, IList<Vector3f> vertices, IEnumerable<Light> lights, Matrix4x4 orientation)
        {
            var v0 = vertices[triangle.V0];
            var v1 = vertices[triangle.V1];
            var v2 = vertices[triangle.V2];

            Vector3f normal = MathHelper.ComputeTriangleNormal(v0, v1, v2);
            Vector3f center = (v0 + v1 + v2) / 3.0f;
            if (-center * normal < 0)
            {
                return;
            }
            var p0 = PointToCanvas(v0);
            var p1 = PointToCanvas(v1);
            var p2 = PointToCanvas(v2);

            _triangleRasterizer.DrawShadedTriangle
            (
                p0,
                p1,
                p2,
                new float[3]
                {
                    v0.Z,
                    v1.Z,
                    v2.Z
                },
                new Vector3f[3]
                {
                    triangle.N0,
                    triangle.N1,
                    triangle.N2
                },
                lights,
                orientation,
                triangle.Color
            );
            _trianglesRendered++;
        }

        private Vector2i PointToCanvas(Vector3f point)
        {
            var viewportPoint = PointToViewport(point);
            int x = (int)((viewportPoint.X * _canvas.Width) / _viewport.Width);
            int y = (int)((viewportPoint.Y * _canvas.Height) / _viewport.Height);
            return new Vector2i(x, y);
        }

        private Vector3f PointToViewport(Vector3f point)
        {
            var x = (point.X * _viewport.Distance) / point.Z;
            var y = (point.Y * _viewport.Distance) / point.Z;
            return new Vector3f(x, y, _viewport.Distance);
        }
    }
}