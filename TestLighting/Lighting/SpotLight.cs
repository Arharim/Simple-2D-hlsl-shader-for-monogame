using Microsoft.Xna.Framework;

namespace TestLighting.Lighting
{
    public struct SpotLight
    {
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Color;
        public float Radius;
        public float Opacity;
        public float Angle;

        public SpotLight(
            Vector3 position,
            Vector3 direction,
            Vector3 color,
            float radius,
            float opacity,
            float angle
        )
        {
            Position = position;
            Direction = direction;
            Color = color;
            Radius = radius;
            Opacity = opacity;
            Angle = angle;
        }
    }
}
