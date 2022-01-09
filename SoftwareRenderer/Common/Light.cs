namespace SoftwareRenderer.Common
{
    public enum LightType
    {
        Directional,
        Point,
        Ambient
    }

    public class Light
    {
        public LightType Type { get; set; }
        public float Intensity { get; set; }
        public Vector3f Position { get; set; }

        public Light(Vector3f pos, float intensity, LightType type)
        {
            Type = type;
            Intensity = intensity;
            Position = pos;
        }

        public static Light CreateAmbient(float intensity) => new Light(new Vector3f(), intensity, LightType.Ambient);
        public static Light CreatePoint(float intensity, Vector3f pos) => new Light(pos, intensity, LightType.Point);
        public static Light CreateDirectional(float intensity, Vector3f dir) => new Light(dir.Normalize(), intensity, LightType.Directional);
    }
}