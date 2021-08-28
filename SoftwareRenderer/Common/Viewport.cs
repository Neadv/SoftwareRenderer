namespace SoftwareRenderer.Common
{
    public class Viewport
    {
        public float Width { get; }
        public float Height { get; }
        public float Distance { get; }

        public Viewport(float width, float height, float distance)
        {
            Width = width;
            Height = height;
            Distance = distance;
        }
    }
}