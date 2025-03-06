using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;
using System;
using UniversalText.UI;

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
            Debug.Log("Added search point of type: " + searchPointConfig.GetType().Name);
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
