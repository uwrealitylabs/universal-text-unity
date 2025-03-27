using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniversalText.Core
{
    public interface ISearchPoint
    {
        /// <summary>
        /// A description of the SearchPoint's context
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Fetches all GameObjects within the context of this SearchPoint
        /// An empty list implies that the search point should be omitted from the RTR
        /// </summary>
        List<UniversalTextTag> Search();
    }
}