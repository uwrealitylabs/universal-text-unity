using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    public class NearbySearchPointConfig : SearchPointConfig
    {
        public Camera xrCamera;
        public float maxDetectionDistance = 10f;
        public int maxDetectedAmount = 3;

        public override ISearchPoint CreateSearchPoint()
        {
            return new NearbySearchPoint(xrCamera, maxDetectionDistance, maxDetectedAmount);
        }
    }
}

