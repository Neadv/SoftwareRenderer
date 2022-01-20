using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SoftwareRenderer.Common;

namespace SoftwareRenderer.Utils
{
    public class CanvasSaver
    {
        private string _resultPath;

        public CanvasSaver(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new System.ArgumentException($"\"{nameof(path)}\" не может быть пустым или содержать только пробел.", nameof(path));
            }

            _resultPath = path;
        }

        public void Save(ICanvas canvas)
        {
            byte[] rgbaBytes = canvas.Bytes;
            using (var image = SixLabors.ImageSharp.Image.LoadPixelData<Rgba32>(rgbaBytes, canvas.Width, canvas.Height))
            {
                image.SaveAsPng(Path.Combine(Directory.GetCurrentDirectory(), _resultPath));
            }
        }
    }
}