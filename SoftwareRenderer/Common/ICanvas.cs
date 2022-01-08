namespace SoftwareRenderer.Common
{
    public interface ICanvas: IBuffer<Color>
    {
        byte[] Bytes { get; }
    }
}