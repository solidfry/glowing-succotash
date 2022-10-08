using System.Collections.Generic;
using UnityEngine;

namespace WaveFunctionCollapse.Inputs
{
    public interface IInputReader<T>
    {
        IValue<T>[][] ReadInputToGrid();
    }
}
