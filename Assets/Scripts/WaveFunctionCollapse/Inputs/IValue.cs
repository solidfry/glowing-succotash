using System;
using System.Collections.Generic;

namespace WaveFunctionCollapse.Inputs
{
    public interface IValue<T> : IEqualityComparer<IValue<T>>, IEquatable<IValue<T>> 
        // IEquatable allows you to compare an object with others of it's type as it reads all the child keys and compares them
        // IEqualityComparer is used in a link library. We can check if cells in output are solved or not
    {
        T Value { get; }
    }
}
