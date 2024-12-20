using LightningSample.Input;
using LightningSample.Lighting;
using LightningSample.Render;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LightningSample
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
            _lightManager.CreateAndAddSpotLight(
                new Vector3(600, 400, 20),
                new Vector3(-1, 0, 0),
                new Vector3(0.9f, 0.7f, 0.3f),
                300f,
                0.8f,
                MathHelper.ToRadians(15),
                MathHelper.ToRadians(30)
            );

            _lightManager.CreateAndAddSpotLight(
                new Vector3(1000, 600, 20),
                new Vector3(1, 0, 0),
                new Vector3(0.3f, 0.7f, 0.9f),
                300f,
                0.8f,
                MathHelper.ToRadians(20),
                MathHelper.ToRadians(40)
            );
        }

        protected override void LoadContent()
        {
            _renderManager.LoadContent(
                Content,
                "Textures/cell",
                "Textures/cell_normal",
                "Shaders/LightingPoints",
                "Shaders/LightingSpots"
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
