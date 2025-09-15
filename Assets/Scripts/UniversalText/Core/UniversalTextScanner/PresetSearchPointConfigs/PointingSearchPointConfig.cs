using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;

namespace UniversalText.UI
{
    /// <summary>
    /// SearchPointConfig for PointingSearchPoint
    /// </summary>
    public class PointingSearchPointConfig : SearchPointConfig
    {
        /// <summary>
        /// User's right Oculus XR hand
        /// </summary>
        public Hand rightHand;
        /// <summary>
        /// User's left Oculus XR hand
        /// </summary>
        public Hand leftHand;
        /// <summary>
        /// Max distance that a gameobject can be detected at
        /// </summary>
        public float maxPointDistance = 30f;

        public override ISearchPoint CreateSearchPoint()
        {
            return new PointingSearchPoint(rightHand, leftHand, maxPointDistance);
        }
    }
}

