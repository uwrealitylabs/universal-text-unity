using UnityEngine;
using TMPro;
using UniversalText.Core;

public class UniversalTextPrompter : MonoBehaviour
{
    [SerializeField] private TextMeshPro displayText;
    [SerializeField] private float updateInterval = 1f;
    private float nextUpdateTime;

    private void Start()
    {
        Debug.Log("UniversalTextPrompter: Starting...");
        if (displayText == null)
        {
            // Try to get TextMeshPro component from this GameObject
            displayText = GetComponent<TextMeshPro>();
            if (displayText == null)
            {
                Debug.LogError("TextMeshPro component not found!");
            }
        }
    }

    private void Update()
    {
        // Update RTR display at regular intervals
        if (Time.time >= nextUpdateTime)
        {
            DisplayRTR();
            nextUpdateTime = Time.time + updateInterval;
        }
    }

    private void DisplayRTR()
    {
        // Get the current RTR from UniversalTextScanner
        string rtr = UniversalTextScanner.Instance.Generate();
        Debug.Log($"Current RTR: {rtr}");

        // Display on screen
        displayText.text = string.IsNullOrEmpty(rtr) 
            ? "No RTR generated - nothing detected in environment" 
            : rtr;
    }
}
