using SoftwareRenderer.Common;
using SoftwareRenderer.Rasterizer.Models;
using SoftwareRenderer.Utils;
using System;

namespace SoftwareRenderer.Rasterizer.SceneBuilder
{
    public class ModelOptionsBuilder
    {
        public Model Instance => new Model(_mesh, _position, _orientation, _scale) { Texture = _texture };

        private Mesh _mesh = null;
        private Image _texture = null;
        private Vector3f _position = new Vector3f(0);
        private Matrix4x4 _orientation = Matrix4x4.Identity;
        private float _scale = 1.0f;

        public ModelOptionsBuilder SetMesh(Mesh mesh)
        {
            _mesh = mesh;
            return this;
        }

        public ModelOptionsBuilder SetCube(float a, Color color)
        {
            _mesh = new CubeMesh(a, color);
            return this;
        }

        public ModelOptionsBuilder SetSphere(float radius, Color color)
        {
            _mesh = new SphereMesh(radius, color);
            return this;
        }

        public ModelOptionsBuilder SetPosition(Vector3f position)
        {
            _position = position;
            return this;
        }

        public ModelOptionsBuilder SetScale(float scale)
        {
            _scale = scale;
            return this;
        }

        public ModelOptionsBuilder SetOrientation(Matrix4x4 orientation)
        {
            _orientation = orientation;
            return this;
        }

        public ModelOptionsBuilder SetRotations(float x, float y, float z)
        {
            throw new NotImplementedException();
        }

        public ModelOptionsBuilder SetTexture(Image texture)
        {
            _texture = texture;
            return this;
        }

        public ModelOptionsBuilder SetTextureFromFile(string path)
        {
            _texture = ImageHelper.LoadImageFromFile(path);
            return this;
        }
    }
}
