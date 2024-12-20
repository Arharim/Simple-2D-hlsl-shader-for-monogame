using System.Collections.Generic;
using LightningSample.Lighting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LightningSample.Render
{
    public class ShaderManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private Effect _pointShader;
        private Effect _spotShader;
        private Matrix _worldMatrix;
        private Vector3 _ambientColor = new Vector3(0.15f);

        public ShaderManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _worldMatrix = Matrix.CreateTranslation(Vector3.Zero);
        }

        public void LoadShader(ContentManager content, string shaderPathP, string shaderPathS)
        {
            _pointShader = content.Load<Effect>(shaderPathP);
            _spotShader = content.Load<Effect>(shaderPathS);
        }

        public Effect GetPointShader()
        {
            return _pointShader;
        }

        public Effect GetSpotShader()
        {
            return _spotShader;
        }

        public void PreparePointLightShader(List<Light> lightGroup, Texture2D normalMap)
        {
            if (_pointShader == null)
                return;

            int count = lightGroup.Count;
            var positions = new Vector3[count];
            var colors = new Vector3[count];
            var radii = new float[count];
            var opacities = new float[count];

            for (int i = 0; i < count; i++)
            {
                positions[i] = lightGroup[i].Position;
                colors[i] = lightGroup[i].Color;
                radii[i] = lightGroup[i].Radius;
                opacities[i] = lightGroup[i].Opacity;
            }

            _pointShader.Parameters["LightPositions"].SetValue(positions);
            _pointShader.Parameters["LightColors"].SetValue(colors);
            _pointShader.Parameters["LightRadii"].SetValue(radii);
            _pointShader.Parameters["LightOpacities"].SetValue(opacities);
            _pointShader.Parameters["LightCount"].SetValue(count);
            _pointShader.Parameters["AmbientColor"].SetValue(_ambientColor);
            _pointShader.Parameters["World"].SetValue(_worldMatrix);
            _pointShader
                .Parameters["ViewProjection"]
                .SetValue(
                    Matrix.CreateOrthographicOffCenter(
                        0,
                        _graphicsDevice.Viewport.Width,
                        _graphicsDevice.Viewport.Height,
                        0,
                        -1,
                        0
                    ) * _worldMatrix
                );
            _pointShader.Parameters["NormalTexture"].SetValue(normalMap);

            _pointShader.CurrentTechnique.Passes[0].Apply();
        }

        public void PrepareSpotLightShader(List<SpotLight> spotLightGroup, Texture2D normalMap)
        {
            if (_spotShader == null)
                return;

            int count = spotLightGroup.Count;
            var positions = new Vector3[count];
            var directions = new Vector3[count];
            var colors = new Vector3[count];
            var radii = new float[count];
            var opacities = new float[count];
            var spotAngles = new float[count];

            for (int i = 0; i < count; i++)
            {
                positions[i] = spotLightGroup[i].Position;
                directions[i] = Vector3.Normalize(spotLightGroup[i].Direction);
                colors[i] = spotLightGroup[i].Color;
                radii[i] = spotLightGroup[i].Radius;
                opacities[i] = spotLightGroup[i].Opacity;
                spotAngles[i] = spotLightGroup[i].OuterConeAngle;
            }

            _spotShader.Parameters["LightPositions"].SetValue(positions);
            _spotShader.Parameters["LightDirections"].SetValue(directions);
            _spotShader.Parameters["LightColors"].SetValue(colors);
            _spotShader.Parameters["LightRadii"].SetValue(radii);
            _spotShader.Parameters["LightOpacities"].SetValue(opacities);
            _spotShader.Parameters["SpotAngles"].SetValue(spotAngles);
            _spotShader.Parameters["LightCount"].SetValue(count);
            _spotShader.Parameters["AmbientColor"].SetValue(_ambientColor);
            _spotShader.Parameters["World"].SetValue(_worldMatrix);
            _spotShader
                .Parameters["ViewProjection"]
                .SetValue(
                    Matrix.CreateOrthographicOffCenter(
                        0,
                        _graphicsDevice.Viewport.Width,
                        _graphicsDevice.Viewport.Height,
                        0,
                        -1,
                        0
                    ) * _worldMatrix
                );
            _spotShader.Parameters["NormalTexture"].SetValue(normalMap);

            _spotShader.CurrentTechnique.Passes[0].Apply();
        }

        public void PrepareShaderForLightGroup(
            List<Light> lightGroup,
            List<SpotLight> spotLightGroup,
            Texture2D normalMap
        )
        {
            int count = lightGroup.Count;
            var positions = new Vector3[count];
            var colors = new Vector3[count];
            var radii = new float[count];
            var opacities = new float[count];

            for (int i = 0; i < count; i++)
            {
                positions[i] = lightGroup[i].Position;
                colors[i] = lightGroup[i].Color;
                radii[i] = lightGroup[i].Radius;
                opacities[i] = lightGroup[i].Opacity;
            }

            _pointShader.Parameters["LightPositions"].SetValue(positions);
            _pointShader.Parameters["LightColors"].SetValue(colors);
            _pointShader.Parameters["LightRadii"].SetValue(radii);
            _pointShader.Parameters["LightOpacities"].SetValue(opacities);
            _pointShader.Parameters["LightCount"].SetValue(count);
            _pointShader.Parameters["AmbientColor"].SetValue(_ambientColor);
            _pointShader.Parameters["World"].SetValue(_worldMatrix);
            _pointShader
                .Parameters["ViewProjection"]
                .SetValue(
                    Matrix.CreateOrthographicOffCenter(
                        0,
                        _graphicsDevice.Viewport.Width,
                        _graphicsDevice.Viewport.Height,
                        0,
                        -1,
                        0
                    ) * _worldMatrix
                );
            _pointShader.Parameters["NormalTexture"].SetValue(normalMap);

            PreparePointLightShader(lightGroup, normalMap);
            PrepareSpotLightShader(spotLightGroup, normalMap);

            _pointShader.CurrentTechnique.Passes[0].Apply();
        }
    }
}
