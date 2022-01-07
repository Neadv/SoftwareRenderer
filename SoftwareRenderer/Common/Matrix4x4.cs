namespace SoftwareRenderer.Common
{
    public struct Matrix4x4
    {
        public static readonly Matrix4x4 Identity = new Matrix4x4(
            new(1, 0, 0, 0),
            new(0, 1, 0, 0),
            new(0, 0, 1, 0),
            new(0, 0, 0, 1)
        );

        private float[,] _matrix;

        public Matrix4x4(int value = 0)
        {
            _matrix = new float[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    _matrix[i, j] = value;
                }
            }
        }

        public Matrix4x4(float[,] data)
        {
            if (data.GetLength(0) != 4 || data.GetLength(1) != 4)
                throw new System.ArgumentException(nameof(data));

            _matrix = data;
        }

        public Matrix4x4(Vector4f v1, Vector4f v2, Vector4f v3, Vector4f v4) : this()
        {
            _matrix = new float[4, 4];

            this[0] = v1;
            this[1] = v2;
            this[2] = v3;
            this[3] = v4;
        }

        public float this[int x, int y]
        {
            get => _matrix[x, y];
            set => _matrix[x, y] = value;
        }

        public Vector4f this[int index]
        {
            get => new Vector4f(_matrix[index, 0], _matrix[index, 1], _matrix[index, 2], _matrix[index, 3]);
            set
            {
                _matrix[index, 0] = value.X;
                _matrix[index, 1] = value.Y;
                _matrix[index, 2] = value.Z;
                _matrix[index, 3] = value.W;
            }
        }

        public Vector4f Multiple(Vector4f vec4)
        {
            var result = new float[4];
            var vec = new float[4] { vec4.X, vec4.Y, vec4.Z, vec4.W };

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i] += _matrix[i, j] * vec[j];
                }
            }

            return new Vector4f(result[0], result[1], result[2], result[3]);
        }

        public Vector3f Multiple(Vector3f vec3)
        {
            var tmp = Multiple(new Vector4f(vec3, 1));
            return new Vector3f(tmp.X, tmp.Y, tmp.Z);
        }

        public Matrix4x4 Multiple(Matrix4x4 matrix)
        {
            var result = new Matrix4x4(0);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        result[i, j] += _matrix[i, k] * matrix[k, j];
                    }
                }
            }

            return result;
        }

        public Matrix4x4 Transpose()
        {
            var result = new Matrix4x4(0);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    result[i, j] += _matrix[j, i];
                }
            }

            return result;
        }

        public static Vector4f operator *(Matrix4x4 matrix4X4, Vector4f vec4) => matrix4X4.Multiple(vec4);
        public static Vector3f operator *(Matrix4x4 matrix4X4, Vector3f vec3) => matrix4X4.Multiple(vec3);
        public static Matrix4x4 operator *(Matrix4x4 matA, Matrix4x4 matB) => matA.Multiple(matB);
    }
}
