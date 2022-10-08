using Enums;
using UnityEngine;

namespace WaveFunctionCollapse.Patterns
{
    public class PatternData
    {
        private Pattern pattern;
        private int frequency;
        private float frequencyRelative;
        private float frequencyRelativeLog2;

        public Pattern Pattern => pattern;
        public float FrequencyRelative => frequencyRelative;
        public float FrequencyRelativeLog2 => frequencyRelativeLog2;

        public PatternData(Pattern pattern)
        {
            this.pattern = pattern;
        }

        public void AddToFrequency() => frequency++;
        public void CalculateRelativeFrequency(int total)
        {
            frequencyRelative = (float)frequency / total;
            frequencyRelativeLog2 = Mathf.Log(frequencyRelative, 2);
        }

        public bool CompareGrid(Direction dir, PatternData data)
        {
            return pattern.ComparePatternToAnotherPattern(dir, data.Pattern);
        }
    }
}