using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    /// <summary>
    /// SearchPointConfig for NearbySearchPoint
    /// </summary>
    public class NearbySearchPointConfig : SearchPointConfig
    {
        /// <summary>
        /// Camera representing the user's position in space
        /// </summary>
        public Camera xrCamera;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        public float maxDetectionDistance = 10f;
        /// <summary>
        /// Max gameobjects that can be detected as "nearby". Closest gameobjects prioritized
        /// </summary>
        public int maxDetectedAmount = 3;

        public override ISearchPoint CreateSearchPoint()
        {
            return new NearbySearchPoint(xrCamera, maxDetectionDistance, maxDetectedAmount);
        }
    }
}

