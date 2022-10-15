using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse.Core;
using WaveFunctionCollapse.Inputs;
using WaveFunctionCollapse.Output;
using WaveFunctionCollapse.Patterns;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
#if UNITY_EDITOR
        [FormerlySerializedAs("input")] public Tilemap inputTilemap;

        [FormerlySerializedAs("output")] public Tilemap outputTilemap;
        public int patternSize;
        public int maxIteration = 500;
        public int outputWidth = 5;
        public int outputHeight = 5;
        public bool equalWeights = false;
        private ValuesManager<TileBase> valuesManager;
        private WFCCore core;
        private PatternManager manager;
        private TilemapOutput output;

        private void Start()
        {
            CreateWfc();
        }

        public void CreateWfc()
        {
            InputReader reader = new(inputTilemap);
            var grid = reader.ReadInputToGrid();
            valuesManager = new(grid);
            manager = new(patternSize);
            manager.ProcessGrid(valuesManager, equalWeights);
            core = new(outputWidth, outputHeight,maxIteration, manager);
        }

        public void CreateTilemap()
        {
            output = new(valuesManager, outputTilemap);
            var result = core.CreateOutputGrid();
            output.CreateOutput(manager, result, outputWidth, outputHeight);
        }

        public void SaveTilemap()
        {
            if (output.OutputImage != null)
            {
                outputTilemap = output.OutputImage;
                GameObject objectToSave = outputTilemap.gameObject;

                PrefabUtility.SaveAsPrefabAsset(objectToSave, "Assets/Saved/output.prefab");
            }
        }
#endif
    }
}