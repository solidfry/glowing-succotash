using System.Collections.Generic;
using System.Linq;
using Enums;
using UnityEngine;
using WaveFunctionCollapse.Patterns;

namespace WaveFunctionCollapse.Core
{
    public class CoreHelper
    {
        private float totalFrequency = 0, totalFrequencyLog = 0;
        private PatternManager patternManager;

        public CoreHelper(PatternManager manager)
        {
            patternManager = manager;
//            for (int i = 0; i < patternManager.GetNumberOfPatterns(); i++)
//            {
//                totalFrequency += patternManager.GetPatternFrequency(i);
//            }
//            totalFrequencyLog = Mathf.Log(totalFrequency, 2);
        }

        public int SelectSolutionPatternFromFrequency(List<int> possibleValues)
        {
            List<float> valueFrequenciesFractions = GetListOfWeightsFromIndices(possibleValues);
            float randomValue = Random.Range(0, valueFrequenciesFractions.Sum());
            float sum = 0;
            int index = 0;
            foreach (var item in valueFrequenciesFractions)
            {
                sum += item;
                if (randomValue <= sum)
                {
                    return index;
                }

                index++;
            }
            return index - 1;
        }

        private List<float> GetListOfWeightsFromIndices(List<int> possibleValues)
        {
            var valueFrequencies = possibleValues.Aggregate(new List<float>(), (acc, val) =>
            {
              acc.Add(patternManager.GetPatternFrequency(val));
              return acc;
            }, acc => acc).ToList();
            return valueFrequencies;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinates, Vector2Int previousCell)
        {
            List<VectorPair> list = new()
            {
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(1, 0), Direction.Right, previousCell),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(-1, 0), Direction.Left, previousCell),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, 1), Direction.Up, previousCell),
                new VectorPair(cellCoordinates, cellCoordinates + new Vector2Int(0, -1), Direction.Down, previousCell),
            };
            return list;
        }

        public List<VectorPair> Create4DirectionNeighbours(Vector2Int cellCoordinate)
        {
            return Create4DirectionNeighbours(cellCoordinate, cellCoordinate);
        }

        public float CalculateEntropy(Vector2Int position, OutputGrid outputGrid)
        {
            float sum = 0;
            foreach (var possibleIndex in outputGrid.GetPossibleValueForPosition(position))
            {
                totalFrequency += patternManager.GetPatternFrequency(possibleIndex);
                sum += patternManager.GetPatternFrequencyLog2(possibleIndex);
            }
            totalFrequencyLog = Mathf.Log(totalFrequency, 2);
            return totalFrequencyLog - (sum / totalFrequency);
        }

        public List<VectorPair> CheckIfNeighboursAreCollapsed(VectorPair pairToCheck, OutputGrid outputGrid)
        {
            return Create4DirectionNeighbours(pairToCheck.CellToPropagatePosition, pairToCheck.BaseCellPosition)
                .Where(
                    x => outputGrid.CheckIfValidPosition(x.CellToPropagatePosition)
                     && 
                     outputGrid.CheckIfCellIsCollapsed(x.CellToPropagatePosition) == false
                     ).ToList();
        }

        public bool CheckCellSolutionForCollision(Vector2Int cellCoordinates, OutputGrid outputGrid)
        {
            foreach (var neighbour in Create4DirectionNeighbours(cellCoordinates))
            {
                if(outputGrid.CheckIfValidPosition(neighbour.CellToPropagatePosition) == false) continue;
                
                HashSet<int> possibleIndices = new();
                foreach (var patternIndexAtNeighbour in outputGrid.GetPossibleValueForPosition(neighbour.CellToPropagatePosition))
                {
                    var possibleNeighboursForBase =
                        patternManager.GetPossibleNeighboursForPatternInDictionary(patternIndexAtNeighbour,
                            neighbour.DirectionFromBase.GetOppositeDirectionTo());
                    possibleIndices.UnionWith(possibleNeighboursForBase);
                }

                if (possibleIndices.Contains(outputGrid.GetPossibleValueForPosition(cellCoordinates).First()) == false)
                {
                    return true;
                }
            }
            return false;
        }

    }
}