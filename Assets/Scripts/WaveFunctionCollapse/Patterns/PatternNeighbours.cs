using System.Collections.Generic;
using Enums;

namespace WaveFunctionCollapse.Patterns
{
    public class PatternNeighbours
    {
        public Dictionary<Direction, HashSet<int>> directionPatternNeighbourDictionary = new();

        public void AddPatternToDictionary(Direction dir, int patternIndex)
        {
            if (directionPatternNeighbourDictionary.ContainsKey(dir))
            {
                directionPatternNeighbourDictionary[dir].Add(patternIndex);
            }
            else
            {
                directionPatternNeighbourDictionary.Add(dir, new HashSet<int>() { patternIndex });
            }
        }

        internal HashSet<int> GetNeighboursInDirection(Direction dir)
        {
            if (directionPatternNeighbourDictionary.ContainsKey(dir))
            {
                return directionPatternNeighbourDictionary[dir];
            }

            return new HashSet<int>();
        }

        public void AddNeighbour(PatternNeighbours neighbours)
        {
            foreach (var item in neighbours.directionPatternNeighbourDictionary)
            {
                if (directionPatternNeighbourDictionary.ContainsKey(item.Key) == false)
                {
                    directionPatternNeighbourDictionary.Add(item.Key, new HashSet<int>());
                }
                else
                {
                    directionPatternNeighbourDictionary[item.Key].UnionWith(item.Value);
                }
            }
        }
    }
}