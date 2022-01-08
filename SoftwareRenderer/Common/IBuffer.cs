namespace SoftwareRenderer.Common
{
    public interface IBuffer<T>
    {
        int Width { get; }
        int Height { get; }
        T this[int x, int y] { get; set; }
        void Set(int x, int y, T value);
        void Set(Vector2i point, T value);
        T Get(int x, int y);
        T Get(Vector2i point);
    }
}
