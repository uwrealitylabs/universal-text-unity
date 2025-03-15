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
/// Test script for Llama3 functionality from UniversalTextManager
/// </summary>
public class Llama3TestScript : MonoBehaviour
{
    [SerializeReference] public List<SearchPointConfig> searchPointConfigs = new List<SearchPointConfig>();
    
    [Header("Llama 3 Settings")]
    [SerializeField] private bool useLlama3Enhancement = true; // Set to true by default for testing
    [SerializeField] private string llama3BaseUrl = "http://localhost:11434"; // Ollama's default port
    
    [Header("Debug Settings")]
    [SerializeField] private float logInterval = 1.0f;
    [SerializeField] private bool debugLogResults = true;
    
    void Start()
    {
        // Set Llama3 base URL since enhancement is enabled
        SetLlama3BaseUrl(llama3BaseUrl);
        
        InitSearchPoints();
        StartCoroutine(PrintUTS());
    }

    private void InitSearchPoints()
    {
        Debug.Log($"Initializing {searchPointConfigs.Count} search points");
        foreach (SearchPointConfig searchPointConfig in searchPointConfigs)
        {
            UniversalTextScanner.Instance.AddSearchPoint(searchPointConfig.CreateSearchPoint());
        }
    }

    private void SetLlama3BaseUrl(string url)
    {
        Debug.Log($"Setting Llama3 base URL to: {url}");
        UniversalTextScanner.Instance.SetLlama3BaseUrl(url);
    }

    private IEnumerator PrintUTS()
    {
        Debug.Log("Starting PrintUTS coroutine");
        while (true)
        {
            if (useLlama3Enhancement)
            {
                Debug.Log("Using Llama3 enhancement");
                // Use async method with coroutine
                StartCoroutine(PrintEnhancedUTS());
            }
            else
            {
                string rawText = UniversalTextScanner.Instance.Generate();
                if (debugLogResults)
                {
                    Debug.Log($"Raw text: {rawText}");
                }
            }
            yield return new WaitForSeconds(logInterval);
        }
    }

    private IEnumerator PrintEnhancedUTS()
    {
        Debug.Log("Starting enhanced text generation");
        Task<string> enhancedTextTask = UniversalTextScanner.Instance.GenerateEnhancedAsync();
        yield return new WaitUntil(() => enhancedTextTask.IsCompleted);
        
        if (enhancedTextTask.Exception != null)
        {
            Debug.LogError($"Error generating enhanced text: {enhancedTextTask.Exception}");
            string fallbackText = UniversalTextScanner.Instance.Generate();
            Debug.Log($"Fallback text: {fallbackText}"); // Fallback to raw output
        }
        else
        {
            if (debugLogResults)
            {
                Debug.Log($"Enhanced: {enhancedTextTask.Result}");
            }
        }
    }

    // For testing without SearchPointConfigs
    public void AddTestSearchPoint(ISearchPoint searchPoint)
    {
        UniversalTextScanner.Instance.AddSearchPoint(searchPoint);
    }
    
    // For manual testing of Llama3 enhancement
    public void TestLlama3WithSampleText(string sampleText)
    {
        Debug.Log($"Testing Llama3 with sample text: {sampleText}");
        StartCoroutine(TestLlama3Enhancement(sampleText));
    }
    
    private IEnumerator TestLlama3Enhancement(string sampleText)
    {
        var llama3Client = new Llama3Client(llama3BaseUrl);
        
        Debug.Log("Sending sample text to Llama3...");
        Task<string> enhancedTextTask = llama3Client.GetEnhancedDescriptionAsync(sampleText);
        yield return new WaitUntil(() => enhancedTextTask.IsCompleted);
        
        if (enhancedTextTask.Exception != null)
        {
            Debug.LogError($"Error testing Llama3: {enhancedTextTask.Exception}");
        }
        else
        {
            Debug.Log($"Llama3 enhancement result: {enhancedTextTask.Result}");
        }
    }
}