using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    /// <summary>
    /// SearchPointConfig for LookingSearchPoint
    /// </summary>
    public class LookingSearchPointConfig : SearchPointConfig
    {
        /// <summary>
        /// Camera representing user's POV (i.e. which direction they are looking in)
        /// </summary>
        public Camera xrCamera;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        public float maxViewDistance = 10f;

        public override ISearchPoint CreateSearchPoint()
        {
            return new LookingSearchPoint(xrCamera, maxViewDistance);
        }
    }
}

