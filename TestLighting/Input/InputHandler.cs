using System;
using System.Collections.Generic;
using LightningSample.Lighting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace LightningSample.Input
{
    public class InputHandler
    {
        private readonly LightManager _lightManager;
        private readonly Dictionary<Keys, Action> _keyActions;
        private bool _isSpotLightMode;

        public InputHandler(LightManager lightManager)
        {
            _lightManager = lightManager;

            _keyActions = new Dictionary<Keys, Action>
            {
                { Keys.Tab, () => _lightManager.SelectNextLight() },
                { Keys.P, () => _lightManager.SelectNextSpotLight() },
                { Keys.Space, ToggleLightMode },
                { Keys.W, () => MoveSelectedLight(0, 0, 1f) },
                { Keys.S, () => MoveSelectedLight(0, 0, -1f) },
                { Keys.Up, () => UpdateLightParameter(10f, "Radius") },
                { Keys.Down, () => UpdateLightParameter(-10f, "Radius") },
                { Keys.Right, () => UpdateLightParameter(0.1f, "Opacity") },
                { Keys.Left, () => UpdateLightParameter(-0.1f, "Opacity") },
                { Keys.Q, () => UpdateSpotAngle(0.1f) },
                { Keys.E, () => UpdateSpotAngle(-0.1f) }
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
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (_isSpotLightMode && _lightManager.SelectedSpotLight != null)
                {
                    var selectedLight = _lightManager.SelectedSpotLight;
                    if (selectedLight.HasValue)
                    {
                        var newDirection =
                            new Vector3(mouse.X, mouse.Y, 0) - selectedLight.Value.Position;
                        _lightManager.UpdateSelectedSpotLightDirection(
                            Vector3.Normalize(newDirection)
                        );
                    }
                }
                else if (_lightManager.SelectedLight != null)
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
        }

        private void MoveSelectedLight(float deltaX, float deltaY, float deltaZ)
        {
            if (_isSpotLightMode && _lightManager.SelectedSpotLight != null)
            {
                var selectedLight = _lightManager.SelectedSpotLight;
                if (!selectedLight.HasValue)
                    return;

                var position = selectedLight.Value.Position;
                _lightManager.UpdateSelectedSpotLightPosition(
                    new Vector3(position.X + deltaX, position.Y + deltaY, position.Z + deltaZ)
                );
            }
            else if (_lightManager.SelectedLight != null)
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

        private void UpdateLightParameter(float delta, string parameter)
        {
            if (_isSpotLightMode && _lightManager.SelectedSpotLight != null)
            {
                var selectedLight = _lightManager.SelectedSpotLight;
                if (!selectedLight.HasValue)
                    return;

                if (parameter == "Radius")
                    _lightManager.UpdateSelectedSpotLightRadius(delta);
                else if (parameter == "Opacity")
                    _lightManager.UpdateSelectedSpotLightOpacity(delta);
            }
            else if (_lightManager.SelectedLight != null)
            {
                var selectedLight = _lightManager.SelectedLight;
                if (!selectedLight.HasValue)
                    return;

                if (parameter == "Radius")
                    _lightManager.UpdateSelectedLightRadius(delta);
                else if (parameter == "Opacity")
                    _lightManager.UpdateSelectedLightOpacity(delta);
            }
        }

        private void UpdateSpotAngle(float delta)
        {
            if (_isSpotLightMode && _lightManager.SelectedSpotLight != null)
            {
                var selectedLight = _lightManager.SelectedSpotLight;
                if (!selectedLight.HasValue)
                    return;

                _lightManager.UpdateSelectedSpotLightAngle(delta);
            }
        }

        private void ToggleLightMode()
        {
            _isSpotLightMode = !_isSpotLightMode;
        }
    }
}
