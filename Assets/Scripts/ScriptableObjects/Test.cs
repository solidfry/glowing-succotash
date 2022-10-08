using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Enums;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse.Inputs;
using WaveFunctionCollapse.Patterns;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        public Tilemap input;
        
        private void Start()
        {
            InputReader reader = new(input);
            var grid = reader.ReadInputToGrid();
//            for (int row = 0; row < grid.Length; row++)
//            {
//                for (int col = 0; col < grid[0].Length; col++)
//                {
//                    Debug.Log($"Row:{row} Col:{col} Tile Name: {grid[row][col].Value.name}");
//                }
//            }

            ValuesManager<TileBase> valueManager = new(grid);
            PatternManager manager = new(1);
            
            manager.ProcessGrid(valueManager, false);
            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Debug.Log(dir.ToString() + " " + string.Join(" ", manager.GetPossibleNeighboursForPatternInDictionary(0,dir).ToArray()));
            }
//            StringBuilder builder = null;
//            List<string> strList = new();
//            
//            for (int row = -1; row <= grid.Length; row++)
//            {
//                builder = new();
//                for (int col = -1; col <= grid[0].Length; col++)
//                {
//                    builder.Append(valuesManager.GetGridValuesIncludingOffset(col, row) + " ");
//                }  
//                strList.Add(builder.ToString());
//            }
//            
//            strList.Reverse();
//            
//            foreach (var item in strList)
//            {
//                Debug.Log(item);
//            }
        }
    }
}