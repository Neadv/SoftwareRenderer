namespace SoftwareRenderer.Common
{
    public interface ICanvas
    {
        byte[] Bytes { get; }
        int Width { get; }
        int Height { get; }
        Color this[int x, int y] { get; set; }
        void SetColor(int x, int y, Color color);
        void SetColor(Vector2i point, Color color);
        Color GetColor(int x, int y);
        Color GetColor(Vector2i point);
    }
}