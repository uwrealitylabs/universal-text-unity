using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniversalText.Core
{
    /// <summary>
    /// Singleton class that generates a text description of the user's current environment and interactions.
    /// Does so by aggregating the UniversalTextTags present in each given SearchPoint
    /// </summary>
    public class UniversalTextScanner
    {
        private static readonly Lazy<UniversalTextScanner> lazy = new Lazy<UniversalTextScanner>(() => new UniversalTextScanner());

        public static UniversalTextScanner Instance { get { return lazy.Value; } }

        private List<ISearchPoint> _searchPoints = new List<ISearchPoint>();

        /// <summary>
        /// Generates RTR by aggregating all search points
        /// </summary>
        public string Generate()
        {
            string rtr = "";
            Debug.Log("# OF SEARCH POINTS: " + _searchPoints.Count);
            foreach (ISearchPoint searchPoint in _searchPoints)
            {
                List<UniversalTextTag> tags = searchPoint.Search().Distinct().ToList();
                if (tags.Count == 0) continue;
                
                if (tags.Count == 1)
                {
                    rtr += $"{searchPoint.Description} {tags[0].ToString()}. ";
                    continue;
                }
                string searchPointStr = searchPoint.Description + ':';
                foreach (UniversalTextTag tag in tags)
                {
                    if (tag == tags.Last())
                    {
                        searchPointStr += $" and {tag.ToString()}. ";
                    }
                    else
                    {
                        searchPointStr += $" {tag.ToString()};";
                    }
                }
                rtr += searchPointStr;
            }
            if (rtr.Length > 0)
            {
                // Remove trailing space
                rtr = rtr.Remove(rtr.Length - 1);
            }
            return rtr;
        }

        /// <summary>
        /// Adds given search point to be included in the RTR
        /// </summary>
        public void AddSearchPoint(ISearchPoint searchPoint)
        {
            _searchPoints.Add(searchPoint);
        }
    }
}

