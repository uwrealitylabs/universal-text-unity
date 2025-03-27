using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniversalText.Core;
using Oculus.Interaction.Input;
using System;
using UniversalText.UI;
using System.Threading.Tasks;

/// <summary>
/// Manages the functionality of the Universal Text package - Should be
/// attached to some persistent GameObject in a scene, such as the OVR Camera Rig.
/// </summary>
public class UniversalTextManager : MonoBehaviour
{
    [SerializeReference] public List<SearchPointConfig> searchPointConfigs = new List<SearchPointConfig>();
    
    [Header("Llama 3 Settings")]
    [SerializeField] private bool useLlama3Enhancement = false;
    [SerializeField] private string llama3BaseUrl = "http://localhost:11434"; // Updated to Ollama's default port
    
    void Start()
    {
        // Set Llama3 base URL if using enhancement
        if (useLlama3Enhancement)
        {
            SetLlama3BaseUrl(llama3BaseUrl);
        }
        
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

    private void SetLlama3BaseUrl(string url)
    {
        
        UniversalTextScanner.Instance.SetLlama3BaseUrl(url);
    }

    private IEnumerator PrintUTS()
    {
        while (true)
        {
            if (useLlama3Enhancement)
            {
                // Use async method with coroutine
                StartCoroutine(PrintEnhancedUTS());
            }
            else
            {
                Debug.Log(UniversalTextScanner.Instance.Generate());
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator PrintEnhancedUTS()
    {
        Task<string> enhancedTextTask = UniversalTextScanner.Instance.GenerateEnhancedAsync();
        yield return new WaitUntil(() => enhancedTextTask.IsCompleted);
        
        if (enhancedTextTask.Exception != null)
        {
            Debug.LogError($"Error generating enhanced text: {enhancedTextTask.Exception}");
            Debug.Log(UniversalTextScanner.Instance.Generate()); // Fallback to raw output
        }
        else
        {
            Debug.Log($"Enhanced: {enhancedTextTask.Result}");
        }
    }
}