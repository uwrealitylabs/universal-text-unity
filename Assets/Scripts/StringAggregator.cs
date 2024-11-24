using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StringAggregator
{
    /// <summary>
    /// Creates the aggregate string representation
    /// </summary>
    /// <returns>Strirng representation of this StringAggregator</returns>
    protected abstract string Aggregate();

    /// <summary>
    /// Retrieves the string representation of this StringAggregator
    /// </summary>
    /// <returns>String representation of this StringAggregator</returns>
    public override sealed string ToString()
    {
        return Aggregate();
    }
}
