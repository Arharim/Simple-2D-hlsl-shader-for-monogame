using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TestLighting.Lighting;

namespace TestLighting.Input
{
    public class InputHandler
    {
        private readonly LightManager _lightManager;
        private readonly Dictionary<Keys, Action> _keyActions;

        public InputHandler(LightManager lightManager)
        {
            _lightManager = lightManager;

            _keyActions = new Dictionary<Keys, Action>
            {
                { Keys.Tab, () => _lightManager.SelectNextLight() },
                { Keys.W, () => MoveSelectedLight(0, 0, 1f) },
                { Keys.S, () => MoveSelectedLight(0, 0, -1f) },
                { Keys.Up, () => _lightManager.UpdateSelectedLightRadius(10f) },
                { Keys.Down, () => _lightManager.UpdateSelectedLightRadius(-10f) },
                { Keys.Right, () => _lightManager.UpdateSelectedLightOpacity(0.1f) },
                { Keys.Left, () => _lightManager.UpdateSelectedLightOpacity(-0.1f) }
            };
        }

        public void HandleInput(MouseState mouse, KeyboardState keyboard)
        {
            foreach (var keyAction in _keyActions)
            {
                if (keyboard.IsKeyDown(keyAction.Key))
                    keyAction.Value.Invoke();
            }

            HandleMouseInput(mouse);
        }

        private void HandleMouseInput(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed && _lightManager.SelectedLight != null)
            {
                var selectedLight = _lightManager.SelectedLight;
                if (selectedLight.HasValue)
                {
                    _lightManager.UpdateSelectedLightPosition(
                        new Vector3(mouse.X, mouse.Y, selectedLight.Value.Position.Z)
                    );
                }
            }
        }

        private void MoveSelectedLight(float deltaX, float deltaY, float deltaZ)
        {
            var selectedLight = _lightManager.SelectedLight;
            if (!selectedLight.HasValue)
                return;

            var position = selectedLight.Value.Position;
            _lightManager.UpdateSelectedLightPosition(
                new Vector3(position.X + deltaX, position.Y + deltaY, position.Z + deltaZ)
            );
        }
    }
}
