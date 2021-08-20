namespace SoftwareRenderer.Common
{
    public interface IRenderer
    {
        void Initialization();
        void Render(ICanvas canvas);
    }
}