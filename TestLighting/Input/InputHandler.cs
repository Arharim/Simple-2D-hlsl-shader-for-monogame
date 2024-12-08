using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestLighting.Lighting;

namespace TestLighting.Input
{
    public class InputHandler
    {
        private readonly LightManager _lightManager;

        public InputHandler(LightManager lightManager)
        {
            _lightManager = lightManager;
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            if (keyboard.IsKeyDown(Keys.Tab))
            {
                _lightManager.SelectNextLight();
            }

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                _lightManager.UpdateSelectedLightPosition(
                    new Vector3(mouse.X, mouse.Y, _lightManager.SelectedLight.Position.Z)
                );
            }

            if (keyboard.IsKeyDown(Keys.W))
            {
                var position = _lightManager.SelectedLight.Position;
                _lightManager.UpdateSelectedLightPosition(
                    new Vector3(position.X, position.Y, position.Z + 1f)
                );
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                var position = _lightManager.SelectedLight.Position;
                _lightManager.UpdateSelectedLightPosition(
                    new Vector3(position.X, position.Y, position.Z - 1f)
                );
            }

            if (keyboard.IsKeyDown(Keys.Up))
                _lightManager.UpdateSelectedLightRadius(10f);
            if (keyboard.IsKeyDown(Keys.Down))
                _lightManager.UpdateSelectedLightRadius(-10f);
            if (keyboard.IsKeyDown(Keys.Right))
                _lightManager.UpdateSelectedLightOpacity(0.1f);
            if (keyboard.IsKeyDown(Keys.Left))
                _lightManager.UpdateSelectedLightOpacity(-0.1f);
        }
    }
}
