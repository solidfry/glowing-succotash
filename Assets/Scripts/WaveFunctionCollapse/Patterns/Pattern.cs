using System;
using System.Collections.Generic;
using System.Xml.Schema;
using Enums;
using Helpers;

namespace WaveFunctionCollapse.Patterns
{
    public class Pattern 
    {
        private int index;
        private int[][] grid;
        public string HashIndex { get; set; }

        public int Index => index;

        public Pattern(int[][] grid, string hashCode, int index)
        {
            this.grid = grid;
            HashIndex = hashCode;
            this.index = index;
        }

        public void SetGridValue(int x, int y, int value) => grid[y][x] = value;

        public int GetGridValue(int x, int y) => grid[y][x];

        public bool CheckValueAtPosition(int x, int y, int value) => value.Equals(GetGridValue(x, y));

        internal bool ComparePatternToAnotherPattern(Direction dir, Pattern pattern)
        {
            int[][] myGrid = GetGridValuesInDirection(dir);
            int[][] otherGrid = pattern.GetGridValuesInDirection(dir.GetOppositeDirectionTo());

            for (int row = 0; row < myGrid.Length; row++)
                for (int col = 0; col < myGrid[0].Length; col++)
                    if (myGrid[row][col] != otherGrid[row][col]) return false;
            
            return true;
        }

        private int[][] GetGridValuesInDirection(Direction dir)
        {
            int[][] gridPartToCompare;
            switch (dir)
            {
                case Direction.Up:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 1, grid.Length, gridPartToCompare);
                    break;
                case Direction.Down:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length - 1, grid.Length);
                    CreatePartOfGrid(0, grid.Length, 0, grid.Length - 1, gridPartToCompare);
                    break;
                case Direction.Left:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(0, grid.Length - 1, 0, grid.Length, gridPartToCompare);
                    break;
                case Direction.Right:
                    gridPartToCompare = MyCollectionExtension.CreateJaggedArray<int[][]>(grid.Length, grid.Length - 1);
                    CreatePartOfGrid(1, grid.Length, 0, grid.Length, gridPartToCompare);
                    break;
                default:
                    return grid;
            }

            return gridPartToCompare;
        }

        private void CreatePartOfGrid(int xMin, int xMax, int yMin, int yMax, int[][] gridPartToCompare)
        {
            List<int> tempList = new();
            for (int row = yMin; row < yMax; row++)
                for (int col = xMin; col < xMax; col++)
                    tempList.Add(grid[row][col]);

            for (int i = 0; i < tempList.Count; i++)
            {
                int x = i % gridPartToCompare.Length;
                int y = i / gridPartToCompare.Length;
                gridPartToCompare[x][y] = tempList[i];
            }
        }
    }
}