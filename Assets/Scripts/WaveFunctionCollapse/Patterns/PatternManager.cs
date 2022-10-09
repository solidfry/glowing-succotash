﻿using System.Collections.Generic;
using Enums;
using Helpers;
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

        public int[][] ConvertPatternToValues<T>(int[][] outputValues)
        {
            int patternOutputWidth = outputValues[0].Length;
            int patternOutputHeight = outputValues.Length;
            int valueGridWidth = patternOutputWidth + patternSize - 1;
            int valueGridHeight = patternOutputHeight + patternSize - 1;
            int[][] valueGrid = MyCollectionExtension.CreateJaggedArray<int[][]>(valueGridHeight, valueGridWidth);
            for (int row = 0; row < patternOutputHeight; row++)
            {
                for (int col = 0; col < patternOutputWidth; col++)
                {
                    Pattern pattern = GetPatternDataFromIndex(outputValues[row][col]).Pattern;
                    GetPatternValues(patternOutputWidth, patternOutputHeight, valueGrid, row, col, pattern);
                }
            }

            return valueGrid;
        }

        private void GetPatternValues(int patternOutputWidth, int patternOutputHeight, int[][] valueGrid, int row, int col, Pattern pattern)
        {
            if (row == patternOutputHeight - 1 && col == patternOutputWidth - 1)
            {
                for (int row1 = 0; row1  < patternSize; row1 ++)
                {
                    for (int col1 = 0; col1 < patternSize; col1++)
                    {
                        valueGrid[row + row1][col + col1] = pattern.GetGridValue(col1, row1);
                    }
                }
            } else if (row == patternOutputHeight - 1)
            {
                for (int row1 = 0; row1 < patternSize; row1++)
                {
                    valueGrid[row + row1][col] = pattern.GetGridValue(0, row1);
                }
            } else if (col == patternOutputWidth - 1)
            {
                for (int col1 = 0; col1 < patternSize; col1++)
                {
                    valueGrid[row][col + col1] = pattern.GetGridValue(col1, 0);
                }
            }
            else
            {
                valueGrid[row][col] = pattern.GetGridValue(0, 0);
            }
        }
    }
}