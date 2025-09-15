using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    /// <summary>
    /// SearchPointConfig for GrabbingSearchPoint
    /// </summary>
    public class GrabbingSearchPointConfig : SearchPointConfig
    {
        /// <summary>
        /// Left Oculus XR HandGrabInteractor
        /// </summary>
        public HandGrabInteractor _leftHandGrabInteractor;
        /// <summary>
        /// Right Oculus XR HandGrabInteractor
        /// </summary>
        public HandGrabInteractor _rightHandGrabInteractor;

        public override ISearchPoint CreateSearchPoint()
        {
            return new GrabbingSearchPoint(_leftHandGrabInteractor, _rightHandGrabInteractor);
        }
    }
}

