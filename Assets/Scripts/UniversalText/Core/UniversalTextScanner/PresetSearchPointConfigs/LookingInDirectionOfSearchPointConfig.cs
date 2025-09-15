using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    /// <summary>
    /// SearchPointConfig for LookingInDirectionOfSearchPoint
    /// </summary>
    public class LookingInDirectionOfSearchPointConfig : SearchPointConfig
    {
        /// <summary>
        /// Camera representing user's POV (i.e. which direction they are looking in)
        /// </summary>
        public Camera xrCamera;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        public float maxViewDistance = 10f;
        /// <summary>
        /// Max spread in degrees from the exact direction the user is looking in within which a gameobject can be detected
        /// </summary>
        public float angleSpread = 15f;

        public override ISearchPoint CreateSearchPoint()
        {
            return new LookingInDirectionOfSearchPoint(xrCamera, maxViewDistance, angleSpread);
        }
    }
}

