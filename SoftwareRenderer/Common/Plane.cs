namespace SoftwareRenderer.Common
{
    public struct Plane
    {
        public Vector3f Normal { get; }
        public float Distance { get; }

        public Plane(Vector3f normal, float distance)
        {
            Normal = normal;
            Distance = distance;
        }

        public Plane(Plane plane)
        {
            Normal = plane.Normal;
            Distance = plane.Distance;
        }
    }
}
