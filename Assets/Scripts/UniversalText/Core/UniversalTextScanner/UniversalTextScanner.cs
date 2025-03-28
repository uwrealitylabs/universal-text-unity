﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private string _llama3BaseUrl = "http://localhost:11434";  // Updated to Ollama's default port
        private Llama3Client _llama3Client;

        // Initialize the Llama3Client
        private UniversalTextScanner() {}

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

        /// <summary>
        /// Adds given search point to be included in the RTR
        /// </summary>
        public void AddSearchPoint(ISearchPoint searchPoint)
        {
            _searchPoints.Add(searchPoint);
        }
    }
}