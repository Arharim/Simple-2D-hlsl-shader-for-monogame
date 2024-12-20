using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LightningSample.Lighting
{
    public class LightManager
    {
        private readonly List<Light> _lights = new List<Light>();
        private readonly List<SpotLight> _spotLights = new List<SpotLight>();
        private int _selectedLightIndex;
        private int _selectedSpotLightIndex;
        private const int MaxLightsPerPass = 6;

        public IReadOnlyList<Light> Lights => _lights;
        public IReadOnlyList<SpotLight> SpotLights => _spotLights;

        public Light? SelectedLight =>
            _lights.Count > 0 ? _lights[_selectedLightIndex] : (Light?)null;

        public SpotLight? SelectedSpotLight =>
            _spotLights.Count > 0 ? _spotLights[_selectedSpotLightIndex] : (SpotLight?)null;

        public Light CreateAndAddLight(Vector3 position, Vector3 color, float radius, float opacity)
        {
            var light = new Light(position, color, radius, opacity);
            _lights.Add(light);
            return light;
        }

        public void AddLight(Light light) => _lights.Add(light);

        public bool RemoveLight(Light light) => _lights.Remove(light);

        public SpotLight CreateAndAddSpotLight(
            Vector3 position,
            Vector3 direction,
            Vector3 color,
            float radius,
            float opacity,
            float innerConeAngle,
            float outerConeAngle
        )
        {
            var spotLight = new SpotLight(
                position,
                direction,
                color,
                radius,
                opacity,
                innerConeAngle,
                outerConeAngle
            );
            _spotLights.Add(spotLight);
            return spotLight;
        }

        public void AddSpotLight(SpotLight spotLight) => _spotLights.Add(spotLight);

        public bool RemoveSpotLight(SpotLight spotLight) => _spotLights.Remove(spotLight);

        public IEnumerable<List<Light>> GetGroupedLights()
        {
            for (int i = 0; i < _lights.Count; i += MaxLightsPerPass)
            {
                yield return _lights.GetRange(i, Math.Min(MaxLightsPerPass, _lights.Count - i));
            }
        }

        public IEnumerable<List<SpotLight>> GetGroupedSpotLights()
        {
            for (int i = 0; i < _spotLights.Count; i += MaxLightsPerPass)
            {
                yield return _spotLights.GetRange(
                    i,
                    Math.Min(MaxLightsPerPass, _spotLights.Count - i)
                );
            }
        }

        public void SelectNextLight()
        {
            if (_lights.Count == 0)
                return;
            _selectedLightIndex = (_selectedLightIndex + 1) % _lights.Count;
        }

        public void SelectNextSpotLight()
        {
            if (_spotLights.Count == 0)
                return;
            _selectedSpotLightIndex = (_selectedSpotLightIndex + 1) % _spotLights.Count;
        }

        public void UpdateSelectedLightPosition(Vector3 position)
        {
            if (_lights.Count == 0)
                return;
            var light = _lights[_selectedLightIndex];
            light.Position = position;
            _lights[_selectedLightIndex] = light;
        }

        public void UpdateSelectedSpotLightPosition(Vector3 position)
        {
            if (_spotLights.Count == 0)
                return;
            var spotLight = _spotLights[_selectedSpotLightIndex];
            spotLight.Position = position;
            _spotLights[_selectedSpotLightIndex] = spotLight;
        }

        public void UpdateSelectedSpotLightDirection(Vector3 direction)
        {
            if (_spotLights.Count == 0)
                return;
            var spotLight = _spotLights[_selectedSpotLightIndex];
            spotLight.Direction = Vector3.Normalize(direction);
            _spotLights[_selectedSpotLightIndex] = spotLight;
        }

        public void UpdateSelectedLightRadius(float delta)
        {
            var light = _lights[_selectedLightIndex];
            light.Radius = Math.Max(10f, light.Radius + delta);
            _lights[_selectedLightIndex] = light;
        }

        public void UpdateSelectedSpotLightRadius(float delta)
        {
            if (_spotLights.Count == 0)
                return;
            var spotLight = _spotLights[_selectedSpotLightIndex];
            spotLight.Radius = Math.Max(10f, spotLight.Radius + delta);
            _spotLights[_selectedSpotLightIndex] = spotLight;
        }

        public void UpdateSelectedLightOpacity(float delta)
        {
            var light = _lights[_selectedLightIndex];
            light.Opacity = Math.Max(0f, light.Opacity + delta);
            _lights[_selectedLightIndex] = light;
        }

        public void UpdateSelectedSpotLightOpacity(float delta)
        {
            if (_spotLights.Count == 0)
                return;
            var spotLight = _spotLights[_selectedSpotLightIndex];
            spotLight.Opacity = Math.Max(0f, spotLight.Opacity + delta);
            _spotLights[_selectedSpotLightIndex] = spotLight;
        }

        public void UpdateSelectedSpotLightAngle(float delta)
        {
            if (_spotLights.Count == 0)
                return;
            var spotLight = _spotLights[_selectedSpotLightIndex];
            spotLight.OuterConeAngle = Math.Max(0f, spotLight.OuterConeAngle + delta);
            _spotLights[_selectedSpotLightIndex] = spotLight;
        }

        public void RemoveSelectedLight()
        {
            if (_lights.Count == 0)
                return;
            _lights.RemoveAt(_selectedLightIndex);
            _selectedLightIndex = Math.Max(0, _selectedLightIndex - 1);
        }

        public void RemoveSelectedSpotLight()
        {
            if (_spotLights.Count == 0)
                return;
            _spotLights.RemoveAt(_selectedSpotLightIndex);
            _selectedSpotLightIndex = Math.Max(0, _selectedSpotLightIndex - 1);
        }

        public void ClearLights()
        {
            _lights.Clear();
            _selectedLightIndex = 0;
        }

        public void ClearSpotLights()
        {
            _spotLights.Clear();
            _selectedSpotLightIndex = 0;
        }
    }
}
