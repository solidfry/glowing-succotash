using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse.Inputs;
using WaveFunctionCollapse.Patterns;

namespace WaveFunctionCollapse.Output
{
    public class TilemapOutput : IOutputCreator<Tilemap>
    {
        private Tilemap outputImage;
        private ValuesManager<TileBase> valuesManager;
        public Tilemap OutputImage => outputImage;

        public TilemapOutput(ValuesManager<TileBase> valueManager, Tilemap outputImage)
        {
            this.outputImage = outputImage;
            this.valuesManager = valueManager;
        }
        
        public void CreateOutput(PatternManager manager, int[][] outputValues, int width, int height)
        {
            if (outputValues.Length == 0) return;
            
            this.outputImage.ClearAllTiles();
            int[][] valueGrid;
            valueGrid = manager.ConvertPatternToValues<TileBase>(outputValues);

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    TileBase tile = (TileBase)this.valuesManager.GetValueFromIndex(valueGrid[row][col]).Value;
                    this.outputImage.SetTile(new Vector3Int(col,row,0), tile);
                }
            }
        }
    }
}