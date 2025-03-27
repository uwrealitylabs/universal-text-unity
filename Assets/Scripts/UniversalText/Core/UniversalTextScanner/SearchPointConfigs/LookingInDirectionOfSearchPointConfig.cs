using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

namespace UniversalText.UI
{
    public class LookingInDirectionOfSearchPointConfig : SearchPointConfig
    {
        public Camera xrCamera;
        public float maxViewDistance = 10f;
        public float angleSpread = 15f;

        public override ISearchPoint CreateSearchPoint()
        {
            return new LookingInDirectionOfSearchPoint(xrCamera, maxViewDistance, angleSpread);
        }
    }
}

