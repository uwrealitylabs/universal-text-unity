using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    public class LookingSearchPointConfig : SearchPointConfig
    {
        public Camera xrCamera;
        public float maxViewDistance = 10f;

        public override ISearchPoint CreateSearchPoint()
        {
            return new LookingSearchPoint(xrCamera, maxViewDistance);
        }
    }
}

