namespace SoftwareRenderer.Common
{
    public interface IRenderer
    {
        void Initialization(ICanvas canvas);
        void Render();
    }
}