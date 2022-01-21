using System;
using System.Diagnostics;
using SoftwareRenderer.Common;
using SoftwareRenderer.RayTracer;
using SoftwareRenderer.Rasterizer;
using SoftwareRenderer.Utils;
using SoftwareRenderer.Rasterizer.SceneBuilder;
using System.Collections.Generic;

namespace SoftwareRenderer
{
    class Program
    {
        public const int Width = 500;
        public const int Height = 500;

        static void Main(string[] args)
        {
            Canvas canvas = new Canvas(Width, Height);
            canvas.Clear(Color.White);


            //var sceneBuilder = new SceneBuilder();

            //sceneBuilder.AddModel()
            //            .SetCube(2, Color.White)
            //            .SetPosition(new Vector3f(-1.5f, 0, 7))
            //            .SetScale(0.75f)
            //            .SetTextureFromFile("Textures/crate-texture.jpg");
            //sceneBuilder.AddModel()
            //            .SetCube(2, Color.White)
            //            .SetPosition(new Vector3f(1.25f, 2.5f, 7.5f))
            //            .SetOrientation(TransformHelper.MakeOYRotationMatrix(195))
            //            .SetTextureFromFile("Textures/wall.jpg");
            //sceneBuilder.AddModel()
            //            .SetSphere(1, Color.Green)
            //            .SetPosition(new Vector3f(1.75f, -0.5f, 7))
            //            .SetScale(1.5f);

            //sceneBuilder.AddLight().SetIntensity(0.2f).SetType(LightType.Ambient);
            //sceneBuilder.AddLight().SetIntensity(0.2f).SetPosition(new Vector3f(-1, 0, 1).Normalize()).SetType(LightType.Directional);
            //sceneBuilder.AddLight().SetIntensity(0.6f).SetPosition(new Vector3f(-3, 3, -10)).SetType(LightType.Point);

            //sceneBuilder.AddCamera().SetPosition(new Vector3f(-3, 1, 2)).SetOrientation(TransformHelper.MakeOYRotationMatrix(-30));

            //Rasterizer.Scene scene = sceneBuilder.Build();

            //SoftwareRasterizer renderer = new SoftwareRasterizer();
            //renderer.Setup(scene, canvas);

            var scene = new RayTracer.Scene
            {
                Spheres = new List<RayTracer.Sphere>
                {
                    new RayTracer.Sphere(
                        pos: new Vector3f(0, -1, 3),
                        r: 1,
                        color: Color.Red,
                        specular: 500,
                        reflective: 0.2f
                    ),
                    new RayTracer.Sphere(
                        pos: new Vector3f(2, 0, 4),
                        r: 1,
                        color: Color.Blue,
                        specular: 500,
                        reflective: 0.3f
                    ),
                    new RayTracer.Sphere(
                        pos: new Vector3f(-2, 0, 4),
                        r: 1,
                        color: Color.Green,
                        specular: 10,
                        reflective: 0.4f
                    ),
                    new RayTracer.Sphere(
                        pos: new Vector3f(0, -5001, 0),
                        r: 5000,
                        color: new Color(255, 255, 0),
                        specular: 1000,
                        reflective: 0.5f
                    )
                },
                Lights = new List<Light>
                {
                    Light.CreateAmbient(0.2f),
                    Light.CreatePoint(0.6f, new Vector3f(2, 1, 0)),
                    Light.CreateDirectional(0.2f, new Vector3f(1, 4, 4))
                }
            };

            var renderer = new SoftwareRayTracer();
            renderer.Setup(scene, canvas);

            var sw = Stopwatch.StartNew();

            renderer.Render();

            sw.Stop();
            Console.WriteLine($"Render: {sw.ElapsedMilliseconds}ms");
            
            sw.Restart();

            CanvasSaver saver = new CanvasSaver("result.png");
            saver.Save(canvas);

            Console.WriteLine($"Save: {sw.ElapsedMilliseconds}ms");
        }
    }
}
