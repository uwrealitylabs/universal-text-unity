using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    public class GrabbingSearchPointConfig : SearchPointConfig
    {
        public HandGrabInteractor _leftHandGrabInteractor;
        public HandGrabInteractor _rightHandGrabInteractor;

        public override ISearchPoint CreateSearchPoint()
        {
            return new GrabbingSearchPoint(_leftHandGrabInteractor, _rightHandGrabInteractor);
        }
    }
}

