using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestLighting.Lighting
{
    public class LightManager
    {
        private readonly List<Light> _lights = new List<Light>();
        private int _selectedLightIndex;

        public IReadOnlyList<Light> Lights => _lights;
        public Light SelectedLight => _lights[_selectedLightIndex];

        public void AddLight(Light light) => _lights.Add(light);

        public void SelectNextLight() =>
            _selectedLightIndex = (_selectedLightIndex + 1) % _lights.Count;

        public void UpdateSelectedLightPosition(Vector3 position)
        {
            var light = _lights[_selectedLightIndex];
            light.Position = position;
            _lights[_selectedLightIndex] = light;
        }

        public void UpdateSelectedLightRadius(float delta)
        {
            var light = _lights[_selectedLightIndex];
            light.Radius = Math.Max(10f, light.Radius + delta);
            _lights[_selectedLightIndex] = light;
        }

        public void UpdateSelectedLightOpacity(float delta)
        {
            var light = _lights[_selectedLightIndex];
            light.Opacity = Math.Max(0f, light.Opacity + delta);
            _lights[_selectedLightIndex] = light;
        }
    }
}
