using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UniversalTextScanner.Instance.Generate();
    }
}
