using Microsoft.Xna.Framework;

namespace LightningSample.Lighting
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

    public struct SpotLight
    {
        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Color;
        public float Radius;
        public float Opacity;
        public float InnerConeAngle;
        public float OuterConeAngle;

        public SpotLight(
            Vector3 position,
            Vector3 direction,
            Vector3 color,
            float radius,
            float opacity,
            float innerConeAngle,
            float outerConeAngle
        )
        {
            Position = position;
            Direction = Vector3.Normalize(direction);
            Color = color;
            Radius = radius;
            Opacity = opacity;
            InnerConeAngle = innerConeAngle;
            OuterConeAngle = outerConeAngle;
        }
    }
}
