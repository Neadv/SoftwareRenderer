using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Runtime.InteropServices;

namespace SoftwareRenderer.Utils
{
    public static class ImageHelper
    {
        public static Common.Image LoadImageFromFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new System.ArgumentException(nameof(path));
            }

            var image = Image.Load<Rgba32>(path);

            byte[] rgbaBytes = null;
            if (image.TryGetSinglePixelSpan(out var pixelSpan))
            {
                rgbaBytes = MemoryMarshal.AsBytes(pixelSpan).ToArray();
            }

            image.Dispose();

            return new Common.Image(rgbaBytes, image.Width, image.Height);
        }

        public static void Save(Common.Image image, string path)
        {
            byte[] rgbaBytes = image.Data;
            using (var saveImage = Image.LoadPixelData<Rgba32>(rgbaBytes, image.Width, image.Height))
            {
                saveImage.SaveAsPng(Path.Combine(Directory.GetCurrentDirectory(), path));
            }
        }
    }
}
