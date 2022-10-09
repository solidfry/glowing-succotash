using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WaveFunctionCollapse.Patterns;

namespace WaveFunctionCollapse.Core
{
    public class CoreSolver
    {
        private PatternManager patternManager;
        private OutputGrid outputGrid;
        private CoreHelper coreHelper;
        private PropagationHelper propagationHelper;

        public CoreSolver(OutputGrid outputGrid, PatternManager patternManager)
        {
            this.outputGrid = outputGrid;
            this.patternManager = patternManager;
            this.coreHelper = new CoreHelper(this.patternManager);
            this.propagationHelper = new PropagationHelper(this.outputGrid, this.coreHelper);
        }

        public void Propagate()
        {
            while (propagationHelper.PairsToPropagate.Count > 0)
            {
                var propagatePair = propagationHelper.PairsToPropagate.Dequeue();
                
                if (propagationHelper.CheckIfPairShouldBeProcessed(propagatePair))
                {
                    ProcessCell(propagatePair);
                }

                if (propagationHelper.CheckForConflicts() || outputGrid.CheckIfGridIsSolved())
                {
                    return;
                }
            }

            if (propagationHelper.CheckForConflicts() && propagationHelper.PairsToPropagate.Count == 0 &&
                propagationHelper.LowEntropySet.Count == 0)
            {
                propagationHelper.SetConflictFlag();
            }
        }

        private void ProcessCell(VectorPair propagatePair)
        {
            if (outputGrid.CheckIfCellIsCollapsed(propagatePair.CellToPropagatePosition))
            {
                propagationHelper.EnqueueUncollapsedNeighbours(propagatePair);
            }
            else
            {
                PropagateNeighbour(propagatePair);
            }
        }

        private void PropagateNeighbour(VectorPair propagatePair)
        {
            var possibleValuesAtNeighbour = outputGrid.GetPossibleValueForPosition(propagatePair.CellToPropagatePosition);
            int startCount = possibleValuesAtNeighbour.Count;

            RemoveImpossibleNeighbours(propagatePair, possibleValuesAtNeighbour);
            int newPossiblePatternCount = possibleValuesAtNeighbour.Count;
            propagationHelper.AnalysePropagationResults(propagatePair, startCount, newPossiblePatternCount);
        }

        private void RemoveImpossibleNeighbours(VectorPair propagatePair, HashSet<int> possibleValuesAtNeighbour)
        {
            HashSet<int> possibleIndices = new();
            foreach (var patternIndexAtBase in outputGrid.GetPossibleValueForPosition(propagatePair.BaseCellPosition))
            {
                var possibleNeighboursForBase =
                    patternManager.GetPossibleNeighboursForPatternInDictionary(patternIndexAtBase,
                        propagatePair.DirectionFromBase);
                possibleIndices.UnionWith(possibleNeighboursForBase);
            }
            possibleValuesAtNeighbour.IntersectWith(possibleIndices);
        }

        public Vector2Int GetLowestEntropyCell()
        {
            if (propagationHelper.LowEntropySet.Count <= 0)
            {
                return outputGrid.GetRandomCell();
            }
            else
            {
                var lowestEntropyElement = propagationHelper.LowEntropySet.First();
                Vector2Int returnVector = lowestEntropyElement.Position;
                propagationHelper.LowEntropySet.Remove(lowestEntropyElement);
                return returnVector;
            }
        }

       public void CollapseCell(Vector2Int cellCoordinates)
       {
           var possibleValue = outputGrid.GetPossibleValueForPosition(cellCoordinates).ToList();
           if (possibleValue.Count == 0 || possibleValue.Count == 1)
           {
               return;
           }
           else
           {
               int index = coreHelper.SelectSolutionPatternFromFrequency(possibleValue);
               outputGrid.SetPatternOnPosition(cellCoordinates.x, cellCoordinates.y, possibleValue[index]);
           }

           if (coreHelper.CheckCellSolutionForCollision(cellCoordinates, outputGrid) == false)
           {
               propagationHelper.AddNewPairsToPropagateQueue(cellCoordinates, cellCoordinates);
           }
           else
           {
               propagationHelper.SetConflictFlag();
           }
       }

       public bool CheckIfSolved() => outputGrid.CheckIfGridIsSolved();

       public bool CheckForConflicts() => propagationHelper.CheckForConflicts();

    }
}