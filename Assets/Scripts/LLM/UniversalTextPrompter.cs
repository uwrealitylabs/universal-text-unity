using UnityEngine;
using TMPro;
using UniversalText.Core;

public class UniversalTextPrompter : MonoBehaviour
{
    [SerializeField] private DeepseekTestPrompt messageSender;
    [SerializeField] private float updateInterval = 1f;
    private float nextUpdateTime;
    private string currentRTR = "";  // Store current RTR for context

    private void Start()
    {
        Debug.Log("UniversalTextPrompter: Starting...");
        if (messageSender == null)
        {
            messageSender = FindObjectOfType<DeepseekTestPrompt>();
            if (messageSender == null)
            {
                Debug.LogError("MessageSender not found!");
            }
        }
    }

    private void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRTR();
            nextUpdateTime = Time.time + updateInterval;
        }
    }

    private void UpdateRTR()
    {
        currentRTR = UniversalTextScanner.Instance.Generate();
        Debug.Log($"Current RTR: {currentRTR}");
    }

    public string GetCurrentContext()
    {
        return currentRTR;
    }
}
