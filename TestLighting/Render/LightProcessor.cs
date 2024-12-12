using System;
using System.Collections.Generic;
using System.Linq;
using TestLighting.Lighting;

namespace TestLighting.Render
{
    public class LightProcessor
    {
        private readonly int _maxLightsPerPass;

        public LightProcessor(int maxLightsPerPass)
        {
            _maxLightsPerPass = maxLightsPerPass;
        }

        public List<List<Light>> SplitLightsIntoGroups(List<Light> lights)
        {
            return Enumerable
                .Range(0, (int)Math.Ceiling((double)lights.Count / _maxLightsPerPass))
                .Select(i => lights.Skip(i * _maxLightsPerPass).Take(_maxLightsPerPass).ToList())
                .ToList();
        }
    }
}
