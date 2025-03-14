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

        // Base URL for Llama 3 instance 
        private readonly string _llama3BaseUrl = "http://localhost:8000";
        private Llama3Client _llama3Client;

        // Initialize the Llama3Client
        private UniversalTextScanner()
        {
            _llama3Client = new Llama3Client(_llama3BaseUrl);
        }

        /// <summary>
        /// Generates RTR by aggregating all search points
        /// </summary>
        public string Generate()
        {
            string rtr = "";
            foreach (ISearchPoint searchPoint in _searchPoints)
            {
                List<UniversalTextTag> tags = searchPoint.Search().Distinct().ToList();
                if (tags.Count == 0) continue;
                
                if (tags.Count == 1)
                {
                    rtr += $"{searchPoint.Description} {tags[0].ToString()}. ";
                    continue;
                }
                string searchPointStr = searchPoint.Description;
                foreach (UniversalTextTag tag in tags)
                {
                    if (tag == tags.Last())
                    {
                        searchPointStr += $" and {tag.ToString()}. ";
                    }
                    else
                    {
                        searchPointStr += $", {tag.ToString()}";
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

         /// 
        /// Generates an enhanced RTR by sending raw RTR to Llama 3
        /// 
        public async Task<string> GenerateEnhancedAsync()
        {
            // Get the raw RTR output
            string rawRtr = Generate();

            // Use Llama 3 to enhance the description
            string enhancedRtr = await _llama3Client.GetEnhancedDescriptionAsync(rawRtr);
            return enhancedRtr;
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

