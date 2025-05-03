using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniversalText.Core;
using System.Text.RegularExpressions;

public class SimpleTextPrompter : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI responseText;
    
    [Header("LLM Handlers")]
    [SerializeField] private DeepSeekHandler deepSeekHandler;
    [SerializeField] private Llama3Handler llama3Handler;
    [SerializeField] private bool useDeepSeek = true;  // Toggle between handlers
    
    [Header("Context")]
    [SerializeField] private UniversalTextPrompter prompter;
    [SerializeField] private bool includeContext = true;

    private void Start()
    {
        Debug.Log("SimpleTextPrompter: Starting...");
        
        if (inputField == null) Debug.LogError("Input Field not assigned!");
        if (button == null) Debug.LogError("Button not assigned!");
        if (responseText == null) Debug.LogError("Response Text not assigned!");
        
        // Find LLM handlers if not assigned
        if (deepSeekHandler == null && useDeepSeek)
        {
            deepSeekHandler = FindObjectOfType<DeepSeekHandler>();
            if (deepSeekHandler == null) Debug.LogError("DeepSeekHandler not found!");
        }
        
        if (llama3Handler == null && !useDeepSeek)
        {
            llama3Handler = FindObjectOfType<Llama3Handler>();
            if (llama3Handler == null) Debug.LogError("Llama3Handler not found!");
        }
        
        if (prompter == null && includeContext)
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

        string fullPrompt = userMessage;
        
        // Add context if enabled
        if (includeContext && prompter != null)
        {
            string context = prompter.GetCurrentContext();
            fullPrompt = $"You are a direct and helpful assistant. Based on this context: {context}\n" +
                         $"Answer this question concisely: {userMessage}";
        }
        
        Debug.Log($"Sending message: {fullPrompt}");

        button.interactable = false;
        inputField.text = "";
        inputField.interactable = false;

        // Choose which handler to use
        if (useDeepSeek && deepSeekHandler != null)
        {
            deepSeekHandler.SendMessage(fullPrompt, OnResponseReceived);
        }
        else if (llama3Handler != null)
        {
            llama3Handler.SendMessage(fullPrompt, OnResponseReceived);
        }
        else
        {
            Debug.LogError("No LLM handler available!");
            responseText.text = "Error: No LLM handler available";
            button.interactable = true;
            inputField.interactable = true;
        }
    }
    
    private void OnResponseReceived(string response)
    {
        Debug.Log($"Received raw response: {response}");
        if (response != null)
        {
            // Clean up the response
            string cleanResponse = CleanResponse(response);
            
            // Force spaces to be visible by replacing them with visible characters
            Debug.Log($"Response with visible spaces: '{cleanResponse.Replace(" ", "_")}'");
            
            // Check if TextMeshProUGUI settings might be causing the issue
            if (responseText != null)
            {
                // Ensure proper text settings
                responseText.enableWordWrapping = true;
                responseText.wordWrappingRatios = 0.4f;
                responseText.enableKerning = true;
                responseText.extraPadding = true;
                
                // Set the text
                responseText.text = cleanResponse;
                
                // Force layout update
                responseText.ForceMeshUpdate();
            }
        }
        else
        {
            responseText.text = "Error: Failed to get response";
        }
        button.interactable = true;
        inputField.interactable = true;
    }
    
    private string CleanResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
            return "";
        
        Debug.Log($"Original response length: {response.Length}");
        
        // Remove thinking tags and content
        string cleaned = Regex.Replace(
            response, 
            "<think>[\\s\\S]*?</think>", 
            "", 
            RegexOptions.Singleline
        );
        
        // Remove other tags
        cleaned = cleaned
            .Replace("<assistant>", "")
            .Replace("</assistant>", "")
            .Replace("Let me think about this.", "")
            .Replace("Let me help you with that.", "")
            .Trim();
        
        // Force spaces between words by adding spaces after punctuation
        cleaned = Regex.Replace(cleaned, @"([.,!?;:])", "$1 ");
        
        // Add spaces between lowercase followed by uppercase (camelCase detection)
        cleaned = Regex.Replace(cleaned, @"([a-z])([A-Z])", "$1 $2");
        
        // Add spaces between letters and numbers
        cleaned = Regex.Replace(cleaned, @"([a-zA-Z])(\d)", "$1 $2");
        cleaned = Regex.Replace(cleaned, @"(\d)([a-zA-Z])", "$1 $2");
        
        // Normalize multiple spaces to single spaces
        cleaned = Regex.Replace(cleaned, @"\s+", " ");
        
        Debug.Log($"Cleaned response with forced spaces: '{cleaned}'");
        
        return cleaned;
    }
    
    // Public method to switch between models
    public void ToggleModel(bool useDeepSeekModel)
    {
        useDeepSeek = useDeepSeekModel;
        Debug.Log($"Switched to {(useDeepSeek ? "DeepSeek" : "Llama3")} model");
    }
} 