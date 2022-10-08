using System.Collections.Generic;
using Enums;
using WaveFunctionCollapse.Inputs;
using WaveFunctionCollapse.Patterns.Strategies;

namespace WaveFunctionCollapse.Patterns
{
    public class PatternManager
    {
        private Dictionary<int, PatternData> patternDataIndexDictionary;
        private Dictionary<int, PatternNeighbours> patternPossibleNeighboursDictionary;
        private int patternSize = -1;
        IFindNeighbourStrategy strategy;
        IFindNeighbourStrategy strategy2;

        public PatternManager(int patternSize)
        {
            this.patternSize = patternSize;
        }

        public void ProcessGrid<T>(ValuesManager<T> valueManager, bool equalWeights, string strategyName = null)
        {
            NeighbourStrategyFactory strategyFactory = new();
            strategy = strategyFactory.CreateInstance(strategyName == null ? patternSize + "" : strategyName);
            CreatePatterns(valueManager, strategy, equalWeights);
        }

        private void CreatePatterns<T>(ValuesManager<T> valueManager, IFindNeighbourStrategy findNeighbourStrategy, bool equalWeights)
        {
            var patternFinderResult = PatternFinder.GetPatternDataFromGrid(valueManager, patternSize, equalWeights);
            patternDataIndexDictionary = patternFinderResult.PatternIndexDictionary;
            GetPatternNeighbours(patternFinderResult,strategy);
        }

        private void GetPatternNeighbours(PatternDataResults patternFinderResult, IFindNeighbourStrategy findNeighbourStrategy)
        {
            patternPossibleNeighboursDictionary =
                PatternFinder.FindPossibleNeighboursForAllPatterns(findNeighbourStrategy, patternFinderResult);
        }

        public PatternData GetPatternDataFromIndex(int index) => patternDataIndexDictionary[index];

        public HashSet<int> GetPossibleNeighboursForPatternInDictionary(int patternIndex, Direction dir) =>     
            patternPossibleNeighboursDictionary[patternIndex].GetNeighboursInDirection(dir);

        public float GetPatternFrequency(int index) => GetPatternDataFromIndex(index).FrequencyRelative;

        public float GetPatternFrequencyLog2(int index) => GetPatternDataFromIndex(index).FrequencyRelativeLog2;

        public int GetNumberOfPatterns() => patternDataIndexDictionary.Count;
    }
}