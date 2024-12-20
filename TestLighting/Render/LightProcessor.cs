using System;
using System.Collections.Generic;
using System.Linq;
using LightningSample.Lighting;

namespace LightningSample.Render
{
    public class LightProcessor
    {
        private readonly int _maxLightsPerPass;

        public LightProcessor(int maxLightsPerPass)
        {
            _maxLightsPerPass = maxLightsPerPass;
        }

        public List<List<Light>> SplitPointLightsIntoGroups(List<Light> pointLights)
        {
            return SplitLightsIntoGroups(pointLights);
        }

        public List<List<SpotLight>> SplitSpotLightsIntoGroups(List<SpotLight> spotLights)
        {
            return SplitLightsIntoGroups(spotLights);
        }

        public List<List<T>> SplitLightsIntoGroups<T>(List<T> lights)
        {
            return Enumerable
                .Range(0, (int)Math.Ceiling((double)lights.Count / _maxLightsPerPass))
                .Select(i => lights.Skip(i * _maxLightsPerPass).Take(_maxLightsPerPass).ToList())
                .ToList();
        }

        public List<List<Light>> SplitLightsIntoGroups1(List<Light> lights)
        {
            return Enumerable
                .Range(0, (int)Math.Ceiling((double)lights.Count / _maxLightsPerPass))
                .Select(i => lights.Skip(i * _maxLightsPerPass).Take(_maxLightsPerPass).ToList())
                .ToList();
        }
    }
}
