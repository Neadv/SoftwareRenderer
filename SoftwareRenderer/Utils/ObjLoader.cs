using SoftwareRenderer.Common;
using SoftwareRenderer.Rasterizer;
using SoftwareRenderer.Rasterizer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SoftwareRenderer.Utils
{
    public static class ObjLoader
    {
        public static Mesh LoadMeshFromFile(string path)
        {
            var vertices = new List<Vector3f>();
            var normals = new List<Vector3f>();
            var uvs = new List<Vector2f>();
            var triangles = new List<Triangle>();
            using (var sr = new StreamReader(path))
            {
                string newLine = sr.ReadLine();
                while (newLine != null)
                {
                    if (newLine.Length > 0 && newLine[0..2] == "vn")
                    {
                        normals.Add(ParseVector3f(newLine[2..]));
                    }
                    else if (newLine.Length > 0 && newLine[0..2] == "vt")
                    {
                        uvs.Add(ParseVector2f(newLine[2..]));
                    }
                    else if (newLine.Length > 0 && newLine[0] == 'v')
                    {
                        vertices.Add(ParseVector3f(newLine[1..]));
                    }
                    else if (newLine.Length > 0 && newLine[0] == 'f')
                    {
                        triangles.Add(ParseTriangle(newLine[1..]));
                    }
                    newLine = sr.ReadLine();
                }

            }
            return new Mesh(vertices, triangles, normals, uvs);
        }

        private static Vector3f ParseVector3f(string str)
        {
            string[] parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                throw new ArgumentException(nameof(str), "[ObjLoader] Parsed string couldn't parse to vector3f");
            }
            return new Vector3f(
                float.Parse(parts[0], CultureInfo.InvariantCulture),
                float.Parse(parts[1], CultureInfo.InvariantCulture),
                float.Parse(parts[2], CultureInfo.InvariantCulture)
            );
        }

        private static Vector2f ParseVector2f(string str)
        {
            string[] parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2)
            {
                throw new ArgumentException(nameof(str), "[ObjLoader] Parsed string couldn't parse to vector2f");
            }
            return new Vector2f(
                float.Parse(parts[0], CultureInfo.InvariantCulture),
                float.Parse(parts[1], CultureInfo.InvariantCulture)
            );
        }

        private static Triangle ParseTriangle(string str)
        {
            string[] parts = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                throw new ArgumentException(nameof(str), "[ObjLoader] Parsed string couldn't parse to triangle");
            }

            var (v0, vt0, vn0) = GetIndexesFromFacePart(parts[0]);

            var (v1, vt1, vn1) = GetIndexesFromFacePart(parts[1]);

            var (v2, vt2, vn2) = GetIndexesFromFacePart(parts[2]);

            return new Triangle(v0, v1, v2, vn0, vn1, vn2, vt0, vt1, vt2, Color.White);
        }

        private static (int, int, int) GetIndexesFromFacePart(string str)
        {
            string[] f = str.Split('/');
            return (
                int.Parse(f[0]) - 1,
                f[1].Length > 0 ? int.Parse(f[1]) -1 : 0,
                f[2].Length > 0 ? int.Parse(f[2]) -1 : 0
            );
        }
    }
}
