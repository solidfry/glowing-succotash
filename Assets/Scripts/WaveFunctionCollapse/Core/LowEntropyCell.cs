using System;
using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse.Core
{
    public class LowEntropyCell : IComparable<LowEntropyCell>, IEqualityComparer<LowEntropyCell>
    {
        public Vector2Int Position
        {
            get; set;
        }
        public float Entropy { get; set; }
        public float smallEntropyNoise;

        public LowEntropyCell(Vector2Int position, float entropy)
        {
            smallEntropyNoise = UnityEngine.Random.Range(0.00f, 0.005f);
            this.Entropy = entropy + smallEntropyNoise;
            this.Position = position;
        }
        
        public int CompareTo(LowEntropyCell other)
        {
            if (Entropy > other.Entropy) return 1;
            if (Entropy < other.Entropy) return -1;
            return 0;
        }

        public bool Equals(LowEntropyCell cell1, LowEntropyCell cell2) => cell1.Position.x == cell2.Position.x && cell1.Position.y == cell2.Position.y;

        public int GetHashCode(LowEntropyCell obj) => obj.GetHashCode();

        public override int GetHashCode() => Position.GetHashCode();
    }
}