using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;

namespace UniversalText.UI
{
    public class PointingSearchPointConfig : SearchPointConfig
    {
        public Hand rightHand;
        public Hand leftHand;
        public float maxPointDistance = 30f;

        public override ISearchPoint CreateSearchPoint()
        {
            return new PointingSearchPoint(rightHand, leftHand, maxPointDistance);
        }
    }
}

