using System.Collections.Generic;

namespace WaveFunctionCollapse.Patterns.Strategies
{
    public interface IFindNeighbourStrategy
    {
        Dictionary<int, PatternNeighbours> FindNeighbours(PatternDataResults patternFinderResult);
    }
}