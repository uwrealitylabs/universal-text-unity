using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniversalText.Core;

public class SimpleTextPrompter : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI responseText;  // Single text area for response
    [SerializeField] private DeepSeekHandler deepSeekHandler;
    [SerializeField] private UniversalTextPrompter prompter;

    private void Start()
    {
        Debug.Log("SimpleTextPrompter: Starting...");
        
        if (inputField == null) Debug.LogError("Input Field not assigned!");
        if (button == null) Debug.LogError("Button not assigned!");
        if (responseText == null) Debug.LogError("Response Text not assigned!");
        if (deepSeekHandler == null)
        {
            deepSeekHandler = FindObjectOfType<DeepSeekHandler>();
            if (deepSeekHandler == null) Debug.LogError("DeepSeekHandler not found!");
        }
        if (prompter == null)
        {
            prompter = FindObjectOfType<UniversalTextPrompter>();
            if (prompter == null) Debug.LogError("UniversalTextPrompter not found!");
        }

        button.onClick.AddListener(FireMessage);
    }

    public void FireMessage()
    {
        string userMessage = inputField.text;
        if (string.IsNullOrEmpty(userMessage)) return;

        // Get current context from prompter
        string context = prompter.GetCurrentContext();
        
        // Create a more direct prompt format
        string fullPrompt = $"You are a direct and helpful assistant. Based on this context: {context}\n" +
                           $"Answer this question concisely: {userMessage}";
        
        Debug.Log($"Sending message with context: {fullPrompt}");

        button.interactable = false;
        inputField.text = "";
        inputField.interactable = false;

        deepSeekHandler.SendMessage(fullPrompt, response =>
        {
            Debug.Log($"Received response: {response}");
            if (response != null)
            {
                // Clean up common thinking phrases if they appear
                string cleanResponse = response
                    .Replace("Let me think about this.", "")
                    .Replace("Let me help you with that.", "")
                    .Trim();
                responseText.text = cleanResponse;
            }
            else
            {
                responseText.text = "Error: Failed to get response";
            }
            button.interactable = true;
            inputField.interactable = true;
        });
    }
} 