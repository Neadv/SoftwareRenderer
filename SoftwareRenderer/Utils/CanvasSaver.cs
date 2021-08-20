using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.Utils
{
    public class CanvasSaver
    {
        private const string Path = "../result.png";

        public void Save(ICanvas canvas)
        {
            byte[] rgbaBytes = canvas.Bytes;
            using (var image = Image.LoadPixelData<Rgba32>(rgbaBytes, canvas.Width, canvas.Height))
            {
                image.SaveAsPng(Path);
            }
        }
    }
}