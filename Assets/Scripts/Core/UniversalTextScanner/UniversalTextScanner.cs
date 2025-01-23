using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UniversalText.Core
{
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
            foreach (ISearchPoint searchPoint in _searchPoints)
            {
                List<UniversalTextTag> tags = searchPoint.Search();
                if (tags.Count == 0) continue;
                
                if (tags.Count == 1)
                {
                    return searchPoint.Description + tags[0].ToString();
                }
                string searchPointStr = searchPoint.Description;
                foreach (UniversalTextTag tag in tags)
                {
                    if (tag == tags.Last())
                    {
                        searchPointStr += " and" + tag.ToString() + ". ";
                    }
                    else
                    {
                        searchPointStr += ", " + tag.ToString();
                    }
                }
                rtr += searchPointStr;
            }
            return rtr.Remove(rtr.Length - 1); // Remove trailing space
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

