using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;

namespace UniversalText.UI
{
    public class ScanningSearchPointConfig : SearchPointConfig
    {
        public GameObject scanner;
        public override ISearchPoint CreateSearchPoint()
        {
            return new ScanningSearchPoint(scanner);
        }
    }
}

