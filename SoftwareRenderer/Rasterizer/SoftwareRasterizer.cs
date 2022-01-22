using System;
using System.Collections.Generic;
using SoftwareRenderer.Common;
using SoftwareRenderer.Rasterizer.Models;
using SoftwareRenderer.Utils;

namespace SoftwareRenderer.Rasterizer
{
    public class SoftwareRasterizer : IRenderer
    {
        public event Action RenderStarted;
        public event Action RenderFinished;

        public Scene Scene { get; private set; }

        private ICanvas _canvas;
        private ZBuffer _zBuffer;

        private TriangleRasterizer _triangleRasterizer;

        public int TrianglesRendered { get; private set; }

        public void Setup(Scene scene, ICanvas canvas)
        {
            Scene = scene ?? throw new ArgumentNullException(nameof(scene));
            _canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));

            _zBuffer = new ZBuffer(_canvas.Width, _canvas.Height);
            _triangleRasterizer = new TriangleRasterizer(_canvas, _zBuffer, Scene.Camera, Scene.Viewport);
        }

        public void Render()
        {
            if (Scene == null)
            {
                throw new InvalidOperationException("Renderer must be initialized. Call method setup before render");
            }

            TrianglesRendered = 0;
            RenderStarted?.Invoke();
            RenderScene(Scene);
            RenderFinished?.Invoke();
        }

        private void RenderScene(Scene scene)
        {
            var cameraMatrix = Scene.Camera.Orientation.Transpose() * TransformHelper.MakeTranslationMatrix(-Scene.Camera.Position);

            foreach (var instance in scene.Instances)
            {
                var transformed = MeshTransformation.TransformAndClip(instance.Mesh, cameraMatrix * instance.Transform, Scene.Camera.ClippingPlanes);
                if (transformed != null)
                {
                    RenderModel(transformed, scene.Lights, instance.Orientation, instance.Texture);
                }
            }
        }

        private void RenderModel(Mesh mesh, IEnumerable<Light> lights, Matrix4x4 orientation, Image texture)
        {
            foreach (var triangle in mesh.Triangles)
            {
                RenderTriangle(triangle, mesh.Vertices, mesh.Normals, mesh.UVs, lights, orientation, texture);
            }
        }

        private void RenderTriangle(Triangle triangle, IList<Vector3f> vertices, IList<Vector3f> normals, IList<Vector2f> uvs, IEnumerable<Light> lights, Matrix4x4 orientation, Image texture)
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

            float[] zPositions = new float[3] { v0.Z, v1.Z, v2.Z };
            Vector3f[] triangleNormals = new Vector3f[3] 
            { 
                normals[triangle.N0], 
                normals[triangle.N1], 
                normals[triangle.N2] 
            };
            if (texture == null)
            {
                _triangleRasterizer.DrawShadedTriangle
                (
                    p0,
                    p1,
                    p2,
                    zPositions,
                    triangleNormals,
                    lights,
                    orientation,
                    triangle.Color
                );
            }
            else
            {
                Vector2f[] triangleUvs = new Vector2f[3] 
                { 
                    uvs[triangle.UV0], 
                    uvs[triangle.UV1], 
                    uvs[triangle.UV2] 
                };
                _triangleRasterizer.DrawTexturedTriangle
                (
                    p0,
                    p1,
                    p2,
                    zPositions,
                    triangleUvs,
                    texture,
                    triangleNormals,
                    lights,
                    orientation
                );
            }
            TrianglesRendered++;
        }

        private Vector2i PointToCanvas(Vector3f point)
        {
            var viewportPoint = PointToViewport(point);
            int x = (int)((viewportPoint.X * _canvas.Width) / Scene.Viewport.Width);
            int y = (int)((viewportPoint.Y * _canvas.Height) / Scene.Viewport.Height);
            return new Vector2i(x, y);
        }

        private Vector3f PointToViewport(Vector3f point)
        {
            var x = (point.X * Scene.Viewport.Distance) / point.Z;
            var y = (point.Y * Scene.Viewport.Distance) / point.Z;
            return new Vector3f(x, y, Scene.Viewport.Distance);
        }
    }
}