using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly ShaderManager _shaderManager;
        private readonly LightProcessor _lightProcessor;

        private RenderTarget2D _baseSceneRenderTarget;
        private RenderTarget2D _normalMapRenderTarget;
        private RenderTarget2D[] _lightPassRenderTargets;

        private Texture2D _texture;
        private Texture2D _textureNormalMap;

        private const int MaxLightsPerPass = 6;

        public RenderManager(
            GraphicsDevice graphicsDevice,
            SpriteBatch spriteBatch,
            LightManager lightManager
        )
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = spriteBatch;
            _lightManager = lightManager;

            _shaderManager = new ShaderManager(graphicsDevice);
            _lightProcessor = new LightProcessor(MaxLightsPerPass);
        }

        public void LoadContent(
            ContentManager content,
            string texturePath,
            string normalMapPath,
            string shaderPathP
        )
        {
            _texture = content.Load<Texture2D>(texturePath);
            _textureNormalMap = content.Load<Texture2D>(normalMapPath);
            _shaderManager.LoadShader(content, shaderPathP);

            int width = _graphicsDevice.PresentationParameters.BackBufferWidth;
            int height = _graphicsDevice.PresentationParameters.BackBufferHeight;

            _baseSceneRenderTarget = new RenderTarget2D(_graphicsDevice, width, height);
            _normalMapRenderTarget = new RenderTarget2D(_graphicsDevice, width, height);

            int lightGroupCount = (int)
                Math.Ceiling((double)_lightManager.Lights.Count / MaxLightsPerPass);
            _lightPassRenderTargets = new RenderTarget2D[lightGroupCount];
            for (int i = 0; i < lightGroupCount; i++)
            {
                _lightPassRenderTargets[i] = new RenderTarget2D(_graphicsDevice, width, height);
            }
        }

        public void Render()
        {
            RenderScene(_baseSceneRenderTarget, _texture);
            RenderScene(_normalMapRenderTarget, _textureNormalMap);

            _graphicsDevice.SetRenderTarget(null);
            RenderWithMultipleShaders();
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

        public void RenderWithMultipleShaders()
        {
            var lightGroups = _lightProcessor.SplitLightsIntoGroups(_lightManager.Lights.ToList());

            for (int i = 0; i < _lightPassRenderTargets.Length; i++)
            {
                RenderGroup(_lightPassRenderTargets[i], lightGroups[i]);
            }

            CompositeLightPasses();
        }

        private void CompositeLightPasses()
        {
            _graphicsDevice.SetRenderTarget(null);
            _graphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            foreach (var target in _lightPassRenderTargets)
            {
                _spriteBatch.Draw(target, Vector2.Zero, Color.White);
            }

            _spriteBatch.End();
        }

        private void RenderGroup(RenderTarget2D renderTarget, List<Light> lightGroup)
        {
            _graphicsDevice.SetRenderTarget(renderTarget);
            _graphicsDevice.Clear(Color.Black);

            _shaderManager.PrepareShaderForLightGroup(lightGroup, _normalMapRenderTarget);

            _spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.Opaque,
                SamplerState.LinearClamp,
                null,
                null,
                _shaderManager.GetShader()
            );
            _spriteBatch.Draw(_baseSceneRenderTarget, Vector2.Zero, Color.White);
            _spriteBatch.End();
        }

        public void UnloadContent()
        {
            _baseSceneRenderTarget.Dispose();
            _normalMapRenderTarget.Dispose();

            foreach (var target in _lightPassRenderTargets)
            {
                target.Dispose();
            }

            _texture.Dispose();
            _textureNormalMap.Dispose();
        }
    }
}
