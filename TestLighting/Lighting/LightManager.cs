using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TestLighting.Lighting
{
    public class LightManager
    {
        private readonly List<Light> _lights = new List<Light>();
        private int _selectedLightIndex;
        private const int MaxLightsPerPass = 6;

        public IReadOnlyList<Light> Lights => _lights;

        public Light? SelectedLight =>
            _lights.Count > 0 ? _lights[_selectedLightIndex] : (Light?)null;

        public Light CreateAndAddLight(Vector3 position, Vector3 color, float radius, float opacity)
        {
            var light = new Light(position, color, radius, opacity);
            _lights.Add(light);
            return light;
        }

        public void AddLight(Light light) => _lights.Add(light);

        public bool RemoveLight(Light light) => _lights.Remove(light);

        public IEnumerable<List<Light>> GetGroupedLights()
        {
            for (int i = 0; i < _lights.Count; i += MaxLightsPerPass)
            {
                yield return _lights.GetRange(i, Math.Min(MaxLightsPerPass, _lights.Count - i));
            }
        }

        public void SelectNextLight()
        {
            if (_lights.Count == 0)
                return;
            _selectedLightIndex = (_selectedLightIndex + 1) % _lights.Count;
        }

        public void UpdateSelectedLightPosition(Vector3 position)
        {
            if (_lights.Count == 0)
                return;
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

        public void RemoveSelectedLight()
        {
            if (_lights.Count == 0)
                return;
            _lights.RemoveAt(_selectedLightIndex);
            _selectedLightIndex = Math.Max(0, _selectedLightIndex - 1);
        }

        public void ClearLights()
        {
            _lights.Clear();
            _selectedLightIndex = 0;
        }
    }
}
