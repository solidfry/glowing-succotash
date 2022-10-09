using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

namespace WaveFunctionCollapse.Core
{
    public class PropagationHelper
    {
        private OutputGrid outputGrid;
        private CoreHelper coreHelper;
        private bool cellWithNoSolutionPresent;
        private SortedSet<LowEntropyCell> lowEntropySet = new();
        private Queue<VectorPair> pairsToPropagate = new();

        public SortedSet<LowEntropyCell> LowEntropySet => lowEntropySet;

        public Queue<VectorPair> PairsToPropagate => pairsToPropagate;

        public PropagationHelper(OutputGrid outputGrid, CoreHelper coreHelper)
        {
            this.outputGrid = outputGrid;
            this.coreHelper = coreHelper;
        } 

        public bool CheckIfPairShouldBeProcessed(VectorPair propagationPair) =>
            outputGrid.CheckIfValidPosition(propagationPair.CellToPropagatePosition) &&
            propagationPair.AreWeCheckingPreviousCellAgain() == false;

        public void AnalysePropagationResults(VectorPair propagatePair, int startCount, int newPossiblePatternCount)
        {
            if (newPossiblePatternCount > 1 && startCount > newPossiblePatternCount)
            {
                AddNewPairsToPropagateQueue(propagatePair.CellToPropagatePosition, propagatePair.BaseCellPosition);
                AddToLowEntropySet(propagatePair.CellToPropagatePosition);
            }

            if (newPossiblePatternCount == 0)
                cellWithNoSolutionPresent = true;
            
            if (newPossiblePatternCount == 1)
                cellWithNoSolutionPresent = coreHelper.CheckCellSolutionForCollision(propagatePair.CellToPropagatePosition, outputGrid);
        }

        private void AddToLowEntropySet(Vector2Int cellToPropagatePosition)
        {
            var elementOfLowEntropySet = LowEntropySet.Where(x => x.Position == cellToPropagatePosition).FirstOrDefault();

            if (elementOfLowEntropySet == null && outputGrid.CheckIfCellIsCollapsed(cellToPropagatePosition) == false)
            {
                float entropy = coreHelper.CalculateEntropy(cellToPropagatePosition, outputGrid);
                lowEntropySet.Add(new LowEntropyCell(cellToPropagatePosition, entropy));
            }
            else
            {
                lowEntropySet.Remove(elementOfLowEntropySet);
                elementOfLowEntropySet.Entropy = coreHelper.CalculateEntropy(cellToPropagatePosition, outputGrid);
                lowEntropySet.Add(elementOfLowEntropySet);
            }
        }

        public void AddNewPairsToPropagateQueue(Vector2Int cellToPropagatePosition, Vector2Int baseCellPosition)
        {
            var list = coreHelper.Create4DirectionNeighbours(cellToPropagatePosition, baseCellPosition);
            foreach (var item in list)
            {
                pairsToPropagate.Enqueue(item);
            }
        }

        public bool CheckForConflicts() => cellWithNoSolutionPresent;

        public void SetConflictFlag() => cellWithNoSolutionPresent = true;

        public void EnqueueUncollapsedNeighbours(VectorPair propagatePair)
        {
            var uncollapsedNeighbours = coreHelper.CheckIfNeighboursAreCollapsed(propagatePair, outputGrid);
            foreach (var uncollapsed in uncollapsedNeighbours)
            {
                pairsToPropagate.Enqueue(uncollapsed);
            }
        }
    }
}