using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;
using System;
using UniversalText.UI;

/// <summary>
/// Manages the functionality of the Universal Text package - Should be
/// attached to some persistent GameObject in a scene, such as the OVR Camera Rig.
/// </summary>
public class UniversalTextManager : MonoBehaviour
{
    [SerializeReference] public List<SearchPointConfig> searchPointConfigs = new List<SearchPointConfig>();
    
    void Start()
    {
        InitSearchPoints();
        StartCoroutine(PrintUTS()); // <-- TEMP
    }

    private void InitSearchPoints()
    {
        foreach (SearchPointConfig searchPointConfig in searchPointConfigs)
        {
            UniversalTextScanner.Instance.AddSearchPoint(searchPointConfig.CreateSearchPoint());
        }
    }

    private IEnumerator PrintUTS()
    {
        while (true)
        {
            Debug.Log(UniversalTextScanner.Instance.Generate());
            yield return new WaitForSeconds(1);
        }
    }
}
