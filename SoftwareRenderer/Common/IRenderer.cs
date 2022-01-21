using System;

namespace SoftwareRenderer.Common
{
    public interface IRenderer
    {
        public event Action RenderStarted;
        public event Action RenderFinished;

        void Render();
    }
}