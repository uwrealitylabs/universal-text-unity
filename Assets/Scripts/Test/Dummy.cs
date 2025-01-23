using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;

/// <summary>
/// Dummy script to be used for testing 
/// </summary>
public class Dummy : MonoBehaviour
{
    public const int MAX_HEALTH = 100;
    public int health = MAX_HEALTH;

    float delay = 1.0f;
    float time = 0.0f;
    private void Start()
    {
        UniversalTextScanner instance = UniversalTextScanner.Instance;
    }
}
