using Microsoft.Xna.Framework;

namespace TestLighting.Lighting
{
    public struct Light
    {
        public Vector3 Position;
        public Vector3 Color;
        public float Radius;
        public float Opacity;

        public Light(Vector3 position, Vector3 color, float radius, float opacity)
        {
            Position = position;
            Color = color;
            Radius = radius;
            Opacity = opacity;
        }
    }
}
