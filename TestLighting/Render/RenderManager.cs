using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TestLighting.Lighting;

namespace TestLighting.Render
{
    public class RenderManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private readonly LightManager _lightManager;

        private RenderTarget2D _baseSceneRenderTarget;
        private RenderTarget2D _normalMapRenderTarget;

        private Texture2D _texture;
        private Texture2D _textureNormalMap;
        private Effect _normalMapShader;

        private Vector3 _ambienceColor = new Vector3(0.35f);
        private Matrix _worldMatrix;

        public RenderManager(
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch,
            LightManager lightManager
        )
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _lightManager = lightManager;
        }

        public void LoadContent(
            ContentManager content,
            string texturePath,
            string normalMapPath,
            string shaderPath
        )
        {
            _texture = content.Load<Texture2D>(texturePath);
            _textureNormalMap = content.Load<Texture2D>(normalMapPath);
            _normalMapShader = content.Load<Effect>(shaderPath);

            int width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            int height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            _baseSceneRenderTarget = new RenderTarget2D(_graphicsDevice, width, height);
            _normalMapRenderTarget = new RenderTarget2D(_graphicsDevice, width, height);

            _worldMatrix = Matrix.CreateTranslation(-Vector3.Zero);
        }

        public void Render()
        {
            RenderScene(_baseSceneRenderTarget, _texture);
            RenderScene(_normalMapRenderTarget, _textureNormalMap);

            _graphicsDevice.SetRenderTarget(null);
            ApplyLightingShader();
        }

        private void RenderScene(RenderTarget2D renderTarget, Texture2D texture)
        {
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            DrawCenteredTexture(texture);
            _spriteBatch.End();
        }

        private void DrawCenteredTexture(Texture2D texture)
        {
            int screenWidth = _graphicsDevice.Viewport.Width;
            int screenHeight = _graphicsDevice.Viewport.Height;

            Vector2 position = new Vector2(
                (screenWidth - texture.Width) / 2,
                (screenHeight - texture.Height) / 2
            );

            _spriteBatch.Draw(texture, position, Color.White);
        }

        private void ApplyLightingShader()
        {
            var lights = _lightManager.Lights;

            Vector3[] positions = new Vector3[lights.Count];
            Vector3[] colors = new Vector3[lights.Count];
            float[] radii = new float[lights.Count];
            float[] opacities = new float[lights.Count];

            for (int i = 0; i < lights.Count; i++)
            {
                positions[i] = lights[i].Position;
                colors[i] = lights[i].Color;
                radii[i] = lights[i].Radius;
                opacities[i] = lights[i].Opacity;
            }

            _normalMapShader.Parameters["LightPositions"].SetValue(positions);
            _normalMapShader.Parameters["LightColors"].SetValue(colors);
            _normalMapShader.Parameters["LightRadii"].SetValue(radii);
            _normalMapShader.Parameters["LightOpacities"].SetValue(opacities);
            _normalMapShader.Parameters["LightCount"].SetValue(lights.Count);
            _normalMapShader.Parameters["AmbientColor"].SetValue(_ambienceColor);
            _normalMapShader.Parameters["World"].SetValue(_worldMatrix);
            _normalMapShader
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
            _normalMapShader.Parameters["NormalTexture"].SetValue(_normalMapRenderTarget);
            _normalMapShader.CurrentTechnique.Passes[0].Apply();

            _spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.LinearClamp,
                null,
                null,
                _normalMapShader
            );
            _spriteBatch.Draw(_baseSceneRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        public void UnloadContent()
        {
            _baseSceneRenderTarget.Dispose();
            _normalMapRenderTarget.Dispose();
            _texture.Dispose();
            _textureNormalMap.Dispose();
        }
    }
}
