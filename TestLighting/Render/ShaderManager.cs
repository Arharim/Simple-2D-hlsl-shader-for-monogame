using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestLighting.Lighting;

namespace TestLighting.Render
{
    public class ShaderManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private Effect _shader;
        private Matrix _worldMatrix;
        private Vector3 _ambientColor = new Vector3(0.15f);

        public ShaderManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _worldMatrix = Matrix.CreateTranslation(Vector3.Zero);
        }

        public void LoadShader(ContentManager content, string shaderPath)
        {
            _shader = content.Load<Effect>(shaderPath);
        }

        public Effect GetShader()
        {
            return _shader;
        }

        public void PrepareShaderForLightGroup(List<Light> lightGroup, Texture2D normalMap)
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

            _shader.Parameters["LightPositions"].SetValue(positions);
            _shader.Parameters["LightColors"].SetValue(colors);
            _shader.Parameters["LightRadii"].SetValue(radii);
            _shader.Parameters["LightOpacities"].SetValue(opacities);
            _shader.Parameters["LightCount"].SetValue(count);
            _shader.Parameters["AmbientColor"].SetValue(_ambientColor);
            _shader.Parameters["World"].SetValue(_worldMatrix);
            _shader
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
            _shader.Parameters["NormalTexture"].SetValue(normalMap);
            _shader.CurrentTechnique.Passes[0].Apply();
        }
    }
}
