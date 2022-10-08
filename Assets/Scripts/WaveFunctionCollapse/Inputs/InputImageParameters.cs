using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WaveFunctionCollapse.Inputs
{
    public class InputImageParameters
    {
        //this class will extract the tiles from the tilemap that are painted
        
        Vector2Int? bottomRightTileCoords = null;
        Vector2Int? topLeftTileCoords = null;
        BoundsInt inputTileMapBounds;
        TileBase[] inputTileMapTilesArray;
        Queue<TileContainer> stackOfTiles = new();
        private int width = 0, height = 0;
        private Tilemap inputTileMap;

     
        public Queue<TileContainer> StackOfTiles
        {
            get => stackOfTiles;
            set => stackOfTiles = value;
        }
        
        public int Width => width;
        public int Height => height;
        
        public InputImageParameters(Tilemap inputTileMap)
        {
            this.inputTileMap = inputTileMap;
            this.inputTileMapBounds = this.inputTileMap.cellBounds;
            this.inputTileMapTilesArray = this.inputTileMap.GetTilesBlock(this.inputTileMapBounds);
            ExtractNonEmptyTiles();
            VerifyInputTiles();
        }

        private void VerifyInputTiles()
        {
            if (topLeftTileCoords == null || bottomRightTileCoords == null)
                throw new System.Exception("WFC: Input tilemap is empty");

            int minX = bottomRightTileCoords.Value.x,
                maxX = topLeftTileCoords.Value.x,
                minY = bottomRightTileCoords.Value.y,
                maxY = topLeftTileCoords.Value.y;

            width = Math.Abs(maxX - minX) + 1;
            height = Math.Abs(maxY - minY) + 1;

            int tileCount = width * height;

            if(stackOfTiles.Count != tileCount) 
                throw new System.Exception("WFC: Tilemap has empty fields");

            if (stackOfTiles.Any(tile => tile.X > maxX || tile.X < minX || tile.Y > maxY || tile.Y < minY)) 
                throw new System.Exception("WFC: Tilemap image should be a filled rectangle");
        }

        private void ExtractNonEmptyTiles()
        {
            for (int row = 0; row < inputTileMapBounds.size.y; row++) 
                for (int col = 0; col < inputTileMapBounds.size.x; col++)
                {
                    int index = col + (row * inputTileMapBounds.size.x);
                    TileBase tile = inputTileMapTilesArray[index];
                        
                    if (bottomRightTileCoords == null && tile != null) bottomRightTileCoords = new(col,row);
                        
                    if(tile != null)
                    {
                        stackOfTiles.Enqueue(new TileContainer(tile, col, row));
                        topLeftTileCoords = new(col, row);
                    }
                }
        }
    }
}
