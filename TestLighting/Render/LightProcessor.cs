using System;
using System.Collections.Generic;
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
            var groups = new List<List<Light>>();
            for (int i = 0; i < lights.Count; i += _maxLightsPerPass)
            {
                groups.Add(lights.GetRange(i, Math.Min(_maxLightsPerPass, lights.Count - i)));
            }
            return groups;
        }
    }
}
