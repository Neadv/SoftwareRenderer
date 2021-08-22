using System;
using System.Diagnostics;
using SoftwareRenderer.Common;
using SoftwareRenderer.RayTracer;
using SoftwareRenderer.Rasterizer;
using SoftwareRenderer.Utils;

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
            
            IRenderer renderer = new SoftwareRasterizer();
            renderer.Initialization();

            var sw = Stopwatch.StartNew();

            renderer.Render(canvas);

            sw.Stop();
            Console.WriteLine($"Render: {sw.ElapsedMilliseconds}ms");
            
            sw.Restart();

            CanvasSaver saver = new CanvasSaver();
            saver.Save(canvas);

            Console.WriteLine($"Save: {sw.ElapsedMilliseconds}ms");
        }
    }
}
