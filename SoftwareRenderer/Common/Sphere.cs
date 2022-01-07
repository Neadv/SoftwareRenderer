namespace SoftwareRenderer.Common
{
    public struct Sphere
    {
        public Vector3f Center { get; set; }
        public float R { get; set; }

        public Sphere(Vector3f center, float r)
        {
            Center = center; 
            R = r;
        }
    }
}
