using System;
using System.Diagnostics;
using SoftwareRenderer.Interfaces;
using SoftwareRenderer.Utils;

namespace SoftwareRenderer
{
    class Program
    {
        public const int Width = 500;
        public const int Heihgt = 500;

        static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();
            ICanvas canvas = new Canvas(Width, Heihgt);
            
            for (int y = -canvas.Height / 2; y < canvas.Height / 2; y++)
            {
                for (int x = -canvas.Width / 2; x < canvas.Width / 2; x++)
                {
                    var color = new Color((float)(x + canvas.Width / 2) / canvas.Width, (float)(y + canvas.Height / 2) / canvas.Height, 0);
                    canvas.SetColor(x, y, color);
                }
            }

            ICanvasSaver saver = new CanvasSaver();
            saver.Save(canvas);

            sw.Stop();
            Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds}ms");
        }
    }
}
