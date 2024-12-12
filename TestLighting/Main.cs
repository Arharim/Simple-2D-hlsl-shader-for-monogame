using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TestLighting.Input;
using TestLighting.Lighting;
using TestLighting.Render;

namespace TestLighting
{
    public class Main : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        private LightManager _lightManager;
        private RenderManager _renderManager;
        private InputHandler _inputHandler;

        public Main()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
            _graphicsDeviceManager.PreferredBackBufferHeight = 1080;
            _graphicsDeviceManager.ApplyChanges();

            _lightManager = new LightManager();
            _renderManager = new RenderManager(GraphicsDevice, _spriteBatch, _lightManager);
            _inputHandler = new InputHandler(_lightManager);

            InitializeLights();

            base.Initialize();
        }

        private void InitializeLights()
        {
            _lightManager.CreateAndAddLight(
                new Vector3(400, 300, 20),
                new Vector3(1.0f, 0.8f, 0.7f),
                200f,
                5.0f
            );
            _lightManager.CreateAndAddLight(
                new Vector3(400, 300, 20),
                new Vector3(1.0f, 0.8f, 0.7f),
                200f,
                5.0f
            );
            _lightManager.CreateAndAddLight(
                new Vector3(400, 300, 20),
                new Vector3(1.0f, 0.8f, 0.7f),
                200f,
                5.0f
            );
            _lightManager.CreateAndAddLight(
                new Vector3(400, 300, 20),
                new Vector3(1.0f, 0.8f, 0.7f),
                200f,
                5.0f
            );
            _lightManager.CreateAndAddLight(
                new Vector3(400, 300, 20),
                new Vector3(1.0f, 0.8f, 0.7f),
                200f,
                5.0f
            );
            _lightManager.CreateAndAddLight(
                new Vector3(1200, 700, 20),
                new Vector3(0.7f, 0.8f, 1.0f),
                200f,
                5.0f
            );
            _lightManager.CreateAndAddLight(
                new Vector3(1200, 700, 20),
                new Vector3(0.7f, 0.8f, 1.0f),
                200f,
                5.0f
            );
        }

        protected override void LoadContent()
        {
            _renderManager.LoadContent(
                Content,
                "Textures/YOUR_TEXTURE_NAME",
                "Textures/YOUR_TEXTURE_NAME_NORMALMAP",
                "Shaders/LightingPoints"
            );
        }

        protected override void Update(GameTime gameTime)
        {
            _inputHandler.HandleInput(Mouse.GetState(), Keyboard.GetState());
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _renderManager.Render();
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _renderManager.UnloadContent();
            base.UnloadContent();
        }
    }
}
