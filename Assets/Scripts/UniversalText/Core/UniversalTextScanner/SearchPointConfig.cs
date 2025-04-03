using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    /// <summary>
    /// Base class for search point configs. Allows customization of search point constructor
    /// arguments in the unity editor.
    /// </summary>
    [System.Serializable]
    public abstract class SearchPointConfig
    {
        /// <summary>
        /// Called at runtime to create actual searchpoint instance using given config parameters
        /// </summary>
        /// <returns></returns>
        public abstract ISearchPoint CreateSearchPoint();
    }
}