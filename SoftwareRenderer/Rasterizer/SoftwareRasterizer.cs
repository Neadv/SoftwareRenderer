using System.Collections.Generic;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.Rasterizer
{
    public class SoftwareRasterizer : IRenderer
    {
        private ICanvas _canvas;
        private Camera _camera;
        private Viewport _viewport;
        private ZBuffer _zBuffer;

        private TriangleRasterizer _triangleRasterizer;

        public void Initialization(ICanvas canvas)
        {
            _canvas = canvas;
            _zBuffer = new ZBuffer(_canvas.Width, _canvas.Height);
            _triangleRasterizer = new TriangleRasterizer(_canvas, _zBuffer);

            _camera = new Camera();
            _viewport = new Viewport(1, 1, 1);

            _camera.Position = new Vector3f(-3, 1, 2);
            _camera.Orientation = TransformHelper.MakeOYRotationMatrix(-30);
        }

        public void Render()
        {
            Scene scene = new Scene();

            Mesh cube = new Cube(2, Color.Red, Color.Green, Color.Blue, Color.Red, Color.Green, Color.Blue);

            scene.Instances.Add(new Instance(cube, new Vector3f(-1.5f, 0, 7), 0.75f));
            scene.Instances.Add(new Instance(cube, new Vector3f(1.25f, 2.5f, 7.5f), TransformHelper.MakeOYRotationMatrix(195), 1));
            scene.Instances.Add(new Instance(cube, new Vector3f(10, 0, 10)));
            scene.Instances.Add(new Instance(cube, new Vector3f(-10, 0, -10), TransformHelper.MakeOYRotationMatrix(195), 1));

            RenderScene(scene);
        }

        public void RenderScene(Scene scene)
        {
            var cameraMatrix = _camera.Orientation.Transpose() * TransformHelper.MakeTranslationMatrix(-_camera.Position);

            foreach (var instance in scene.Instances)
            {
                var transformed = TransformAndClip(instance.Mesh, cameraMatrix * instance.Transform);
                if (transformed != null)
                {
                    RenderModel(transformed);
                }
            }
        }

        private Mesh TransformAndClip(Mesh mesh, Matrix4x4 transform)
        {
            Vector3f center = transform * mesh.BoundingSphere.Center;
            float radius2 = mesh.BoundingSphere.R * mesh.BoundingSphere.R;
            foreach (var clippedPlane in _camera.ClippingPlanes)
            {
                float distance2 = clippedPlane.Normal.Dot(center) + clippedPlane.Distance;
                if (distance2 < -radius2)
                {
                    return null;
                }
            }

            var transformedMesh = Mesh.Copy(mesh);
            transformedMesh.Transform(transform);
            foreach (var plane in _camera.ClippingPlanes)
            {
                var newTriangles = new List<Triangle>(mesh.Triangles.Count);
                foreach (var triangle in transformedMesh.Triangles)
                {
                    ClipTriangle(triangle, plane, transformedMesh.Vertices, newTriangles);
                }
                transformedMesh.Triangles = newTriangles;
            }

            return transformedMesh;
        }

        private void ClipTriangle(Triangle triangle, Plane plane, List<Vector3f> vertices, IList<Triangle> triangles)
        {
            Vector3f v0 = vertices[triangle.V0];
            Vector3f v1 = vertices[triangle.V1];
            Vector3f v2 = vertices[triangle.V2];

            int inCount = 0;
            inCount += plane.Normal * v0 + plane.Distance > 0 ? 1 : 0;
            inCount += plane.Normal * v1 + plane.Distance > 0 ? 1 : 0;
            inCount += plane.Normal * v2 + plane.Distance > 0 ? 1 : 0;

            if (inCount == 3)
            {
                triangles.Add(triangle);
            }
            else if (inCount == 1)
            {
                // The triangle has one vertex in. Output is one clipped triangle.
            }
            else if (inCount == 2)
            {
                // The triangle has two vertices in. Output is two clipped triangles.
            }
        }

        private void RenderModel(Mesh mesh)
        {
            var projected = new Vector2i[mesh.Vertices.Count];
            var zPositions = new float[mesh.Vertices.Count];
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                projected[i] = PointToCanvas(mesh.Vertices[i]);
                zPositions[i] = mesh.Vertices[i].Z;
            }
            foreach (var triangle in mesh.Triangles)
            {
                RenderTriangle(triangle, projected, zPositions);
            }
        }

        private void RenderTriangle(Triangle triangle, Vector2i[] projected, float[] zPositions)
        {
            _triangleRasterizer.DrawFilledTriangle
            (
                projected[triangle.V0],
                projected[triangle.V1],
                projected[triangle.V2],
                new float[3]
                {
                    zPositions[triangle.V0],
                    zPositions[triangle.V1],
                    zPositions[triangle.V2]
                },
                triangle.Color
            );
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